#if UNITY_EDITOR
using Gob3AQ.ResourceAtlas;
using Gob3AQ.Waypoint.Network;
using Gob3AQ.Waypoint;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gob3AQ.Waypoint.WaypointTool
{
    [EditorTool("Network Waypoint Tool", typeof(WaypointNetwork))]
    public class WaypointNetworkTool : EditorTool
    {
        GUIContent m_ToolbarIcon;
        WaypointNetwork selectedNetwork;

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

            selectedNetwork = (WaypointNetwork)target;
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


            if (GUILayout.Button("Attach all Waypoints", GUILayout.Width(200)))
            {
                Object[] waypoints = FindObjectsByType(typeof(WaypointClass), FindObjectsSortMode.None);
                List<WaypointClass> waypoint_list = new(waypoints.Length);

                for(int i=0;i < waypoints.Length; i++)
                {
                    waypoint_list.Add((WaypointClass)waypoints[i]);
                }

                selectedNetwork.AssignChildren(waypoint_list);

                EditorUtility.SetDirty(selectedNetwork);
                EditorUtility.SetDirty(selectedNetwork.gameObject);
            }

            if(GUILayout.Button("Calculate Paths", GUILayout.Width(200)))
            {
                selectedNetwork.CalculateSolutions();
                EditorUtility.SetDirty(selectedNetwork);
                EditorUtility.SetDirty(selectedNetwork.gameObject);
            }


            Handles.EndGUI();
        }
    }

    [EditorTool("Waypoint Tool", typeof(WaypointClass))]
    public class WaypointTool : EditorTool
    {
        GUIContent m_ToolbarIcon;
        List<WaypointClass> selectedWaypoints;

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




            if (selectedWaypoints.Count == 1)
            {
                WaypointClass selectedWaypoint = selectedWaypoints[0];

                if (GUILayout.Button("Connect New Waypoint", GUILayout.Width(200)))
                {
                    WaypointClass waypoint = CreateWaypoint(selectedWaypoint.transform.position + Vector3.up, selectedWaypoint.name + "_NEW");

                    selectedWaypoint.ConnectWith(waypoint);
                    waypoint.ConnectWith(selectedWaypoint);

                    EditorUtility.SetDirty(waypoint);
                    EditorUtility.SetDirty(selectedWaypoint);
                    EditorUtility.SetDirty(waypoint.gameObject);
                    EditorUtility.SetDirty(selectedWaypoint.gameObject);
                }

                if (GUILayout.Button("Destroy (Isolate)", GUILayout.Width(200)))
                {
                    for (int i = 0; i < selectedWaypoint.ConnectedWaypoints.Count; i++)
                    {
                        WaypointClass otherWaypoint = selectedWaypoint.ConnectedWaypoints[i];

                        selectedWaypoint.DisconnectWith(otherWaypoint);
                        otherWaypoint.DisconnectWith(selectedWaypoint);
                        EditorUtility.SetDirty(otherWaypoint);
                        EditorUtility.SetDirty(otherWaypoint.gameObject);
                    }
                    DestroyImmediate(selectedWaypoint.gameObject);
                }
            }
            else if (selectedWaypoints.Count == 2)
            {
                bool areConnected = false;

                for(int i = 0; i < selectedWaypoints[0].ConnectedWaypoints.Count;i++)
                {
                    if (selectedWaypoints[0].ConnectedWaypoints[i] == selectedWaypoints[1])
                    {
                        areConnected = true;
                        break;
                    }
                }

                if (areConnected)
                {
                    if (GUILayout.Button("Disconnect", GUILayout.Width(200)))
                    {
                        selectedWaypoints[0].DisconnectWith(selectedWaypoints[1]);
                        selectedWaypoints[1].DisconnectWith(selectedWaypoints[0]);
                        EditorUtility.SetDirty(selectedWaypoints[0]);
                        EditorUtility.SetDirty(selectedWaypoints[0].gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[1]);
                        EditorUtility.SetDirty(selectedWaypoints[1].gameObject);
                    }

                    if (GUILayout.Button("Insert New between", GUILayout.Width(200)))
                    {
                        WaypointClass waypoint = CreateWaypoint((selectedWaypoints[0].transform.position + selectedWaypoints[1].transform.position) / 2 + Vector3.up, selectedWaypoints[0].name + "_NEW");

                        selectedWaypoints[0].DisconnectWith(selectedWaypoints[1]);
                        selectedWaypoints[1].DisconnectWith(selectedWaypoints[0]);

                        selectedWaypoints[0].ConnectWith(waypoint);
                        waypoint.ConnectWith(selectedWaypoints[0]);
                        selectedWaypoints[1].ConnectWith(waypoint);
                        waypoint.ConnectWith(selectedWaypoints[1]);

                        EditorUtility.SetDirty(waypoint);
                        EditorUtility.SetDirty(waypoint.gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[0]);
                        EditorUtility.SetDirty(selectedWaypoints[0].gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[1]);
                        EditorUtility.SetDirty(selectedWaypoints[1].gameObject);
                    }
                }
                else
                {
                    if (GUILayout.Button("Connect(Normal)", GUILayout.Width(200)))
                    {
                        selectedWaypoints[0].ConnectWith(selectedWaypoints[1]);
                        selectedWaypoints[1].ConnectWith(selectedWaypoints[0]);
                        EditorUtility.SetDirty(selectedWaypoints[0]);
                        EditorUtility.SetDirty(selectedWaypoints[0].gameObject);
                        EditorUtility.SetDirty(selectedWaypoints[1]);
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
            GameObject prefab = ResourceAtlasClass.GetPrefabForEditor(PrefabForEditorEnum.PREFAB_EDITOR_WAYPOINT);

            GameObject wpgameobject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            wpgameobject.transform.position = position;
            wpgameobject.name = name;

            WaypointClass waypoint = wpgameobject.GetComponent<WaypointClass>();

            return waypoint;
   
        }
    }
}
#endif