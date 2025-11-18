

using System;
using System.Collections.Generic;
using UnityEngine;
using Gob3AQ.LevelMaster;
using Gob3AQ.VARMAP.LevelMaster;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gob3AQ.Waypoint.Network
{
    [System.Serializable]
    public struct WaypointSolution
    {
        [SerializeField]
        private List<int> travelTo;

        public readonly IReadOnlyList<int> TravelTo => travelTo;

        public WaypointSolution(List<int> travelTo)
        {
            this.travelTo = travelTo;
        }
    }

    [System.Serializable]
    public class WaypointNetwork : MonoBehaviour
    {
        [SerializeField]
        private int network_ID;


        [SerializeField]
        [Tooltip("Do not modify, needs to be stored and it is automatically calculated")]
        private List<WaypointClass> children;


        [SerializeField]
        [Tooltip("Do not modify, needs to be stored and it is automatically calculated")]
        private List<WaypointSolution> solutions;


        private void Awake()
        {
            List<Vector2> waypoints_points = new(children.Count);

            for (int i = 0; i < children.Count; ++i)
            {
                waypoints_points.Add(children[i].transform.position);
            }

            /* LevelMaster is ensured to be initialized before this */
            LevelMasterClass.WPListRegister(waypoints_points, solutions);
        }


#if UNITY_EDITOR
        public void AssignChildren(List<WaypointClass> in_children)
        {
            children.Clear();
            children.AddRange(in_children);
            NumerateChildren();
        }
        public void NumerateChildren()
        {
            for(int i=0;i<children.Count;i++)
            {
                WaypointClass child = children[i];
                child.SetID(i);
                child.name = "WN_" + network_ID + "_" + i;
                child.SetNetwork(this);
                EditorUtility.SetDirty(child);
                EditorUtility.SetDirty(child.gameObject);
            }
        }
        public void CalculateSolutions()
        {
            if (children.Count == 0) { return; }

            solutions.Clear();

            /* src,dst - Definitive results */
            float[,] result_distances = new float[children.Count, children.Count];
            List<WaypointClass>[,] result_pathTo = new List<WaypointClass>[children.Count, children.Count];

            for (int i = 0; i < children.Count; i++)
            {
                for (int j = 0; j < children.Count; j++)
                {
                    result_distances[i, j] = float.MaxValue;
                }
            }

            /* Every child will have its chance to propagate */
            foreach(WaypointClass child in children)
            {
                Propagate(child, result_distances, result_pathTo);
            }

            /* Now algorithm has been executed, distances are necessary no more.
             * Transform those lists into elem to follow */
            for (int i = 0; i < children.Count; ++i)
            {
                List<int> travelTo = new(children.Count);

                for (int j = 0; j < children.Count; ++j)
                {
                    List<WaypointClass> path_to = result_pathTo[i, j];
                    int n_elems = path_to.Count;

                    /* Retrieve list to traverse to given elem */
                    if (i == j)
                    {
                        travelTo.Add(path_to[n_elems-1].ID_in_Network);
                    }
                    else
                    {
                        travelTo.Add(path_to[n_elems-2].ID_in_Network);
                    }
                }

                WaypointSolution src_solution = new(travelTo);
                solutions.Add(src_solution);
            }
        }

        private void Propagate(WaypointClass source, float[,] result_distances, List<WaypointClass>[,] result_pathTo)
        {
            Stack<float> accumDistances = new(children.Count);
            Stack<WaypointClass> accumElems = new(children.Count);

            int srcindex = source.ID_in_Network;

            /* Start with self */
            accumDistances.Push(0f);
            accumElems.Push(source);

            while(accumElems.Count > 0)
            {
                /* Get actual inspected elem and accumulated distance from source to here */
                WaypointClass actualInspected = accumElems.Peek();
                float distanceFromSrcToInspected = accumDistances.Peek();

                /* Update distance is less than minimum observed  */
                int dstindex = actualInspected.ID_in_Network;

                result_distances[srcindex, dstindex] = distanceFromSrcToInspected;
                result_pathTo[srcindex, dstindex] = new(accumElems);

                /* If actual inspected elem has unvisited connections, take first found */
                /* Algorithm will take here to this inspected if that branch is consumed */
                /* Then, next connection will be visited, and so on */
                bool found = false;
                foreach(WaypointClass connection in actualInspected.ConnectedWaypoints)
                {
                    /* Calculate distance to this point and add to base distance */
                    float connectionDistance = (connection.transform.position - actualInspected.transform.position).magnitude;
                    float distanceFromSrcToConnected = distanceFromSrcToInspected + connectionDistance;

                    /* Continue on this element if distance promises to have an update */
                    if (distanceFromSrcToConnected < result_distances[srcindex, connection.ID_in_Network])
                    {
                        /* Add to stack, iteration will inspect this one on next loop */
                        accumElems.Push(connection);
                        accumDistances.Push(distanceFromSrcToConnected);
                        found = true;
                        break;
                    }
                }

                /* End with this inspected element, as it has no unvisited elements to traverse to */
                if(!found)
                {
                    _ = accumElems.Pop();
                    _ = accumDistances.Pop();
                }
            }
        }

#endif
    }
}