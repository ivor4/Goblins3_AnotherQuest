using Gob3AQ.ItemMaster;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Gob3AQ.GameElement.Extension.FishermanGrid
{
    /* Not used, but may be useful in the future */
    public class CustomGridClass : MonoBehaviour, IGameElementExtension
    {
        private const int X_SIZE = 6;
        private const int Y_SIZE = 18;
        private const int TOTAL_SIZE = X_SIZE * Y_SIZE;
        private static readonly GridPosition[] DIR_TABLE = new GridPosition[8]
        {
            new GridPosition(-1, -1),
            new GridPosition(0, -1),
            new GridPosition(1, -1),
            new GridPosition(-1, 0),
            new GridPosition(1, 0),
            new GridPosition(-1, 1),
            new GridPosition(0, 1),
            new GridPosition(1, 1)
        };

        private readonly struct GridPosition : IEquatable<int>
        {
            public readonly int x;
            public readonly int y;
            public readonly int hashCode;

            public GridPosition(int x, int y)
            {
                this.x = x;
                this.y = y;
                hashCode = (x * Y_SIZE) + y;
            }

            public GridPosition(int hashCode)
            {
                this.hashCode = hashCode;
                x = hashCode / Y_SIZE;
                y = hashCode % Y_SIZE;
            }

            public bool Equals(int other)
            {
                return hashCode == other;
            }

            public override bool Equals(object obj)
            {
                return obj is int intObj && Equals(intObj);
            }

            public override int GetHashCode()
            {
                return hashCode;
            }
        }

        private struct GridElement
        {
            readonly public bool selectable;
            public bool painted;
            readonly public GridPosition position;

            public GridElement(bool selectable, bool painted, GridPosition position)
            {
                this.selectable = selectable;
                this.painted = painted;
                this.position = position;
            }
        }



        private GridElement[] grid;
        private int[,] solutions;
        private Stack<int> accumElems;
        private Stack<float> accumDistances;
        private List<int>[] tempSolutions;
        private float[] tempDistances;


        public void OnDespawn()
        {
            
        }

        public void OnExtensionDestroy()
        {
            //ItemMasterClass.RemoveItemExtension(ItemExtensionFunction.ITEM_EXTENSION_FN_PAINT_FISHERMEN_GRID);
        }

        public void OnReceiveExtFn(ItemExtensionFunction extFn, in NumberPack numberPack)
        {
            
        }

        public void OnSpawn()
        {
            
        }

        private void Awake()
        {
            grid = new GridElement[TOTAL_SIZE];
            solutions = new int[TOTAL_SIZE, TOTAL_SIZE];
            tempDistances = new float[TOTAL_SIZE];
            accumElems = new Stack<int>(TOTAL_SIZE);
            accumDistances = new Stack<float>(TOTAL_SIZE);
            tempSolutions = new List<int>[TOTAL_SIZE];
            for (int i = 0; i < TOTAL_SIZE; i++)
            {
                tempSolutions[i] = new List<int>(TOTAL_SIZE);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            //ItemMasterClass.AddItemExtension(ItemExtensionFunction.ITEM_EXTENSION_FN_PAINT_FISHERMEN_GRID, this);

            for(int x=0; x < X_SIZE; x++)
            {
                for(int y=0; y < Y_SIZE; y++)
                {
                    GridPosition pos = new GridPosition(x, y);
                    bool isWhite = (x + y) % 2 == 0;
                    bool transitable = isWhite;
                    transitable &= !(x <= 1 && y >= 11);
                    transitable &= !(x <= 2 && y >= 16);
                    grid[pos.hashCode] = new GridElement(isWhite & transitable, !isWhite, pos);
                }
            }

            UpdateSolutions();
        }

        private void UpdateSolutions()
        {
            /* Clear with -1 (not transitable) */
            MemoryMarshal.CreateSpan(ref solutions[0, 0], solutions.Length).Fill(-1);

            for (int hashCode = 0; hashCode < TOTAL_SIZE; hashCode++)
            {
                if (grid[hashCode].selectable && !grid[hashCode].painted)
                {
                    Propagate(hashCode);
                }
            }
        }

        private void Propagate(int srcHashCode)
        {
            /* Start with self */
            int srcindex = srcHashCode;

            foreach (var list in tempSolutions)
            {
                list.Clear();
            }
            MemoryMarshal.CreateSpan(ref tempDistances[0], tempDistances.Length).Fill(float.MaxValue);
            accumDistances.Push(0f);
            accumElems.Push(srcindex);


            while (accumElems.Count > 0)
            {
                /* Get actual inspected elem and accumulated distance from source to here */
                int actualInspected = accumElems.Peek();
                float distanceFromSrcToInspected = accumDistances.Peek();

                /* Update distance is less than minimum observed  */
                int dstindex = actualInspected;

                tempDistances[dstindex] = distanceFromSrcToInspected;
                accumElems.CopyTo(tempSolutions[dstindex]);

                /* If actual inspected elem has unvisited connections, take first found */
                /* Algorithm will take here to this inspected if that branch is consumed */
                /* Then, next connection will be visited, and so on */
                int dstorigx = grid[dstindex].position.x;
                int dstorigy = grid[dstindex].position.y;
                Vector2 vectorOrig = new Vector2(dstorigx, dstorigy);

                bool found = false;

                for (int dir = 0; dir < DIR_TABLE.Length; dir++)
                {
                    int x = dstorigx + DIR_TABLE[dir].x;
                    int y = dstorigy + DIR_TABLE[dir].y;

                    if ((x < 0) || (x >= X_SIZE) || (y < 0) || (y >= Y_SIZE)) continue;

                    GridPosition connectionPos = new GridPosition(x, y);

                    if ((!grid[connectionPos.hashCode].selectable) || grid[connectionPos.hashCode].painted) continue;


                    /* Calculate distance to this point and add to base distance */
                    float connectionDistance = (new Vector2(x,y) - vectorOrig).magnitude;
                    float distanceFromSrcToConnected = distanceFromSrcToInspected + connectionDistance;

                    /* Continue on this element if distance promises to have an update */
                    if (distanceFromSrcToConnected < tempDistances[connectionPos.hashCode])
                    {
                        /* Add to stack, iteration will inspect this one on next loop */
                        accumElems.Push(connectionPos.hashCode);
                        accumDistances.Push(distanceFromSrcToConnected);
                        found = true;
                        break;
                    }
                }

                /* End with this inspected element, as it has no unvisited elements to traverse to */
                if (!found)
                {
                    _ = accumElems.Pop();
                    _ = accumDistances.Pop();
                }
            }

            /* Place in solutions */
            for (int dstIndex = 0; dstIndex < tempSolutions.Length; ++dstIndex)
            {
                List<int> path_to = tempSolutions[dstIndex];
                int solution_index = -1;

                if (path_to.Count > 0)
                {
                    int n_elems = path_to.Count;

                    /* Retrieve list to traverse to given elem */
                    if (srcindex == dstIndex)
                    {
                        solution_index = path_to[n_elems - 1];
                    }
                    else
                    {
                        solution_index = path_to[n_elems - 2];
                    }
                }

                solutions[srcindex, dstIndex] = solution_index;
            }
        }
    }
}
