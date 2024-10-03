#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using Gob3AQ.Waypoint.Types;

namespace Gob3AQ.Waypoint.WaypointTool
{
    [EditorTool("Waypoint Tool", typeof(Waypoint))]
    public class WaypointTool : EditorTool
    {
        GUIContent m_ToolbarIcon;
        List<Waypoint> selectedWaypoints;
        WaypointConnectionType connType;
        WaypointType wpType;

        public override GUIContent toolbarIcon
        {
            get
            {
                if (m_ToolbarIcon == null)
                    m_ToolbarIcon = new GUIContent(
                        AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Sprites/spr_diamond.png"),
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
            selectedWaypoints = new List<Waypoint>(selectedList.Count);
            for (int i = 0; i < selectedList.Count; i++)
            {
                selectedWaypoints.Add((Waypoint)selectedList[i]);
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
                Waypoint selectedWaypoint = selectedWaypoints[0];

                if (GUILayout.Button("Connect New Waypoint", GUILayout.Width(200)))
                {
                    Waypoint waypoint = CreateWaypoint(selectedWaypoint.transform.position + Vector3.up, selectedWaypoint.name + "_NEW");

                    waypoint.PreloadPathPoint = new WaypointPreloadPathPoint() { pathNum = -1, pathPoint = -1 };

                    Waypoint.PreloadWaypointConnection(selectedWaypoint, waypoint, connType);

                    EditorUtility.SetDirty(waypoint.gameObject);
                }

                if (GUILayout.Button("Destroy (Isolate)", GUILayout.Width(200)))
                {
                    for (int i = 0; i < selectedWaypoint.PreloadConnections.Count; i++)
                    {
                        Waypoint otherWaypoint = selectedWaypoint.PreloadConnections[i].withWaypoint;

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
                if (Waypoint.IsPreloadConnectedWith(selectedWaypoints[0], selectedWaypoints[1]))
                {
                    if (GUILayout.Button("Disconnect", GUILayout.Width(200)))
                    {
                        Waypoint.PreloadWaypointDisconnection(selectedWaypoints[0], selectedWaypoints[1]);
                        EditorUtility.SetDirty(selectedWaypoints[0].gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[1].gameObject);
                    }

                    if (GUILayout.Button("Insert New between", GUILayout.Width(200)))
                    {
                        Waypoint waypoint = CreateWaypoint((selectedWaypoints[0].transform.position + selectedWaypoints[1].transform.position) / 2 + Vector3.up, selectedWaypoints[0].name + "_NEW");

                        /* This will belong the same terrain, however, not attached to a path num nor point */
                        waypoint.PreloadPathPoint = new WaypointPreloadPathPoint() { pathNum = -1, pathPoint = -1 };

                        Waypoint.PreloadWaypointDisconnection(selectedWaypoints[0], selectedWaypoints[1]);
                        Waypoint.PreloadWaypointConnection(selectedWaypoints[0], waypoint, connType);
                        Waypoint.PreloadWaypointConnection(waypoint, selectedWaypoints[1], connType);

                        EditorUtility.SetDirty(waypoint.gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[0].gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[1].gameObject);
                    }
                }
                else
                {
                    if (GUILayout.Button("Connect(Normal)", GUILayout.Width(200)))
                    {
                        Waypoint.PreloadWaypointConnection(selectedWaypoints[0], selectedWaypoints[1], connType);
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

        private Waypoint CreateWaypoint(Vector3 position, string name)
        {
            GameObject wpgameobject = Instantiate(Resources.Load<GameObject>("Prefabs/Waypoint")); 
            wpgameobject.transform.position = position;
            wpgameobject.name = name;

            Waypoint waypoint = wpgameobject.GetComponent<Waypoint>();

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
    }
}
#endif