#if UNITY_EDITOR
using Gob3AQ.ResourceAtlas;
using Gob3AQ.Waypoint.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gob3AQ.Waypoint.WaypointTool
{
    [EditorTool("Waypoint Tool", typeof(WaypointClass))]
    public class WaypointTool : EditorTool
    {
        GUIContent m_ToolbarIcon;
        List<WaypointClass> selectedWaypoints;
        WaypointConnectionType connType;
        WaypointType wpType;

        public override GUIContent toolbarIcon
        {
            get
            {
                if (m_ToolbarIcon == null)
                    m_ToolbarIcon = new GUIContent(
                        AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/spr_diamond.png"),
                        "Waypoint Tool");
                return m_ToolbarIcon;
            }
        }

        void OnEnable()
        {
            ToolManager.activeToolChanged += ActiveToolDidChange;
        }

        void OnDisable()
        {
            ToolManager.activeToolChanged -= ActiveToolDidChange;
        }

        void ActiveToolDidChange()
        {
            if (!ToolManager.IsActiveTool(this))
                return;

            List<Object> selectedList = new List<Object>(targets);
            selectedWaypoints = new List<WaypointClass>(selectedList.Count);
            for (int i = 0; i < selectedList.Count; i++)
            {
                selectedWaypoints.Add((WaypointClass)selectedList[i]);
            }
            wpType = WaypointType.WAYPOINT_TYPE_GROUND;
            connType = WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL;
        }

        public override void OnToolGUI(EditorWindow window)
        {
            var evt = Event.current;

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (evt.type == EventType.MouseDown || evt.type == EventType.MouseUp || evt.type == EventType.MouseDrag)
            {
                //evt.Use();
            }

            Handles.BeginGUI();

            #region "ConnectionTypes"
            GUILayout.BeginHorizontal();
            GUILayout.Label("Connection Type", GUILayout.Width(200));

            if (GUILayout.Button(connType == WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL ? ">Normal<" : "Normal", GUILayout.Width(100)))
            {
                connType = WaypointConnectionType.WAYPOINT_CONNECTION_NORMAL;
            }
            else if (GUILayout.Button(connType == WaypointConnectionType.WAYPOINT_CONNECTION_DISCONNECTED ? ">Disconnected<" : "Disconnected", GUILayout.Width(100)))
            {
                connType = WaypointConnectionType.WAYPOINT_CONNECTION_DISCONNECTED;
            }
            else
            {

            }
            GUILayout.EndHorizontal();
            #endregion

            #region "WaypointTypes"
            GUILayout.BeginHorizontal();
            GUILayout.Label("Waypoint Type", GUILayout.Width(200));

            if (GUILayout.Button(wpType == WaypointType.WAYPOINT_TYPE_GROUND ? ">Ground<" : "Ground", GUILayout.Width(100)))
            {
                wpType = WaypointType.WAYPOINT_TYPE_GROUND;
            }
            else
            {

            }

            GUILayout.EndHorizontal();
            #endregion


            if (selectedWaypoints.Count == 1)
            {
                WaypointClass selectedWaypoint = selectedWaypoints[0];

                if (GUILayout.Button("Connect New Waypoint", GUILayout.Width(200)))
                {
                    WaypointClass waypoint = CreateWaypoint(selectedWaypoint.transform.position + Vector3.up, selectedWaypoint.name + "_NEW");

                    WaypointClass.PreloadWaypointConnection(selectedWaypoint, waypoint, connType);

                    EditorUtility.SetDirty(waypoint.gameObject);
                }

                if (GUILayout.Button("Destroy (Isolate)", GUILayout.Width(200)))
                {
                    for (int i = 0; i < selectedWaypoint.PreloadConnections.Count; i++)
                    {
                        WaypointClass otherWaypoint = selectedWaypoint.PreloadConnections[i].withWaypoint;

                        for (int e = 0; e < otherWaypoint.PreloadConnections.Count; e++)
                        {
                            if (otherWaypoint.PreloadConnections[e].withWaypoint == selectedWaypoint)
                            {
                                otherWaypoint.PreloadConnections.RemoveAt(e);
                                e--;
                                EditorUtility.SetDirty(otherWaypoint.gameObject);
                            }
                        }
                    }

                    DestroyImmediate(selectedWaypoint.gameObject);
                }
            }
            else if (selectedWaypoints.Count == 2)
            {
                if (WaypointClass.IsPreloadConnectedWith(selectedWaypoints[0], selectedWaypoints[1]))
                {
                    if (GUILayout.Button("Disconnect", GUILayout.Width(200)))
                    {
                        WaypointClass.PreloadWaypointDisconnection(selectedWaypoints[0], selectedWaypoints[1]);
                        EditorUtility.SetDirty(selectedWaypoints[0].gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[1].gameObject);
                    }

                    if (GUILayout.Button("Insert New between", GUILayout.Width(200)))
                    {
                        WaypointClass waypoint = CreateWaypoint((selectedWaypoints[0].transform.position + selectedWaypoints[1].transform.position) / 2 + Vector3.up, selectedWaypoints[0].name + "_NEW");


                        WaypointClass.PreloadWaypointDisconnection(selectedWaypoints[0], selectedWaypoints[1]);
                        WaypointClass.PreloadWaypointConnection(selectedWaypoints[0], waypoint, connType);
                        WaypointClass.PreloadWaypointConnection(waypoint, selectedWaypoints[1], connType);

                        EditorUtility.SetDirty(waypoint.gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[0].gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[1].gameObject);
                    }
                }
                else
                {
                    if (GUILayout.Button("Connect(Normal)", GUILayout.Width(200)))
                    {
                        WaypointClass.PreloadWaypointConnection(selectedWaypoints[0], selectedWaypoints[1], connType);
                        EditorUtility.SetDirty(selectedWaypoints[0].gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[1].gameObject);
                    }
                }
            }
            else
            {

            }
            Handles.EndGUI();
        }

        private WaypointClass CreateWaypoint(Vector3 position, string name)
        {
            AsyncOperationHandle<GameObject> handle = ResourceAtlasClass.GetPrefab(PrefabEnum.PREFAB_WAYPOINT, out bool success);

            if (success)
            {
                GameObject wpgameobject = Instantiate(handle.Result);
                handle.Release();
                wpgameobject.transform.position = position;
                wpgameobject.name = name;

                WaypointClass waypoint = wpgameobject.GetComponent<WaypointClass>();

                waypoint.PreloadType = wpType;

                SpriteRenderer wprenderer = wpgameobject.GetComponent<SpriteRenderer>();

                switch (wpType)
                {
                    default:
                        wprenderer.color = Color.white;
                        break;
                }

                return waypoint;
            }
            else
            {
                return null;
            }   
        }
    }
}
#endif