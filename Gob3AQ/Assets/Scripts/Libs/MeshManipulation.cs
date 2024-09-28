using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVerse.Libs.Arith;
using MVerse.Libs.Triangulator;

namespace MVerse.Libs.MeshManipulation
{
    public struct VertexTruple
    {
        public Vector3 pos;
        public Vector2 uv;
        public Color col;
        public Vector3 unfoldPos;
        public bool hasUnfolded;


        public VertexTruple(float px, float py, float pz, float uvx, float uvy, Color col)
        {
            pos = new Vector3(px, py, pz);
            uv = new Vector2(uvx, uvy);
            this.col = col;
            hasUnfolded = false;
            unfoldPos = Vector3.zero;
        }

        public VertexTruple(float px, float py, float pz, float uvx, float uvy, Color col, Vector3 unfoldedPos)
        {
            pos = new Vector3(px, py, pz);
            uv = new Vector2(uvx, uvy);
            this.col = col;
            hasUnfolded = true;
            unfoldPos = unfoldedPos;
        }

        public static Vector3[] GetPosArray(List<VertexTruple> list)
        {
            Vector3[] retVal = new Vector3[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                retVal[i] = list[i].pos;
            }

            return retVal;
        }

        public static Vector3[] GetUnfoldPosArray(List<VertexTruple> list)
        {
            Vector3[] retVal = new Vector3[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                retVal[i] = list[i].unfoldPos;
            }

            return retVal;
        }

        public static Vector2[] GetUVArray(List<VertexTruple> list)
        {
            Vector2[] retVal = new Vector2[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                retVal[i] = list[i].uv;
            }

            return retVal;
        }

        public static Color[] GetColorArray(List<VertexTruple> list)
        {
            Color[] retVal = new Color[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                retVal[i] = list[i].col;
            }

            return retVal;
        }
    }
    public static class MeshManipulationClass
    {
        public const ushort ROUNDRESOLUTION = 8;

        public const float ANGLE_0 = 0;
        public const float ANGLE_90 = Mathf.PI / 2;
        public const float ANGLE_180 = Mathf.PI;
        public const float ANGLE_270 = 3 * Mathf.PI / 2;
        public const float ANGLE_360 = 2 * Mathf.PI;

        public static Bounds CreateBoundsFromArray(Vector3[] pointArray)
        {
            Bounds bounds = new Bounds();
            MinMaxVector3 minmax = MinMaxVector3.PreparedStruct;

            for(int i=0;i<pointArray.Length;i++)
            {
                minmax.Witness(pointArray[i]);
            }

            bounds.SetMinMax(minmax.min, minmax.max);

            return bounds;

        }

        public static Bounds CreateBoundsFromArray(Vector2[] pointArray)
        {
            Bounds bounds = new Bounds();
            MinMaxVector3 minmax = MinMaxVector3.PreparedStruct;

            for (int i = 0; i < pointArray.Length; i++)
            {
                minmax.Witness(pointArray[i]);
            }

            bounds.SetMinMax(minmax.min, minmax.max);

            return bounds;

        }


        /// <summary>
        /// Creates a rectangled mesh simmetric with its central point as the pivot, which can be shifted from 0,0. Size normally is used as size in local object coordinates
        /// </summary>
        /// <param name="width">Width in local coordinates</param>
        /// <param name="height">Height in local coordinates</param>
        /// <param name="centerX">Pivot X</param>
        /// <param name="centerY">Pivot Y</param>
        /// <returns></returns>
        public static Mesh CreateSimpleQuad(float width, float height, Color color, float centerX = 0, float centerY = 0, float baseZ = 0)
        {
            Mesh mesh = new Mesh();

            List<VertexTruple> baseTruple = new List<VertexTruple>();
            //Triangulator triangulator;

            Vector3[] posarray;
            Vector2[] uvarray;
            Color[] colorarray;

            /* CCW */
            baseTruple.Add(new VertexTruple(centerX - width / 2, centerY - height / 2, baseZ, 0, 0, color));//0,0
            baseTruple.Add(new VertexTruple(centerX + width / 2, centerY - height / 2, baseZ, 1, 0, color));//1,0
            baseTruple.Add(new VertexTruple(centerX + width / 2, centerY + height / 2, baseZ, 1, 1, color));//1,1
            baseTruple.Add(new VertexTruple(centerX - width / 2, centerY + height / 2, baseZ, 0, 1, color));//0,1



            posarray = VertexTruple.GetPosArray(baseTruple);
            uvarray = VertexTruple.GetUVArray(baseTruple);
            colorarray = VertexTruple.GetColorArray(baseTruple);
#if(false)
            triangulator = new Triangulator(posarray);

            mesh.vertices = posarray;
            mesh.uv = uvarray;
            mesh.colors = colorarray;
            mesh.triangles = triangulator.Triangulate();
#endif

            mesh.UploadMeshData(true);

            return mesh;
        }

        /// <summary>
        /// Create a circle mesh
        /// </summary>
        /// <param name="centerX">centerX in local coordinates, normally 0</param>
        /// <param name="centerY">centerY in local coordinates, normally 0</param>
        /// <param name="radius">Radius in local coordinates</param>
        /// <param name="npoints">Number of vertices to create circle shape, the more, less edgy</param>
        /// <returns></returns>
        public static Mesh CreateCircleMesh(float centerX, float centerY, float radius, int npoints, float baseZ = 0)
        {
            float degincrease = 360f / npoints;
            Mesh mesh = new Mesh();

            List<VertexTruple> baseTruple = new List<VertexTruple>();
            //Triangulator triangulator;

            Vector3[] posarray;
            Vector2[] uvarray;

            for (int i = 0; i < npoints; i++)
            {
                float degrees = degincrease * i;
                float cs = Mathf.Cos(Mathf.Deg2Rad * degrees);
                float sn = Mathf.Sin(Mathf.Deg2Rad * degrees);

                baseTruple.Add(new VertexTruple(centerX + cs * radius, centerY + sn * radius, baseZ, (cs + 1) / 2, (sn + 1) / 2, Color.white));
            }

            posarray = VertexTruple.GetPosArray(baseTruple);
            uvarray = VertexTruple.GetUVArray(baseTruple);
#if(false)
            triangulator = new Triangulator(posarray);

            mesh.vertices = posarray;
            mesh.uv = uvarray;
            mesh.triangles = triangulator.Triangulate();
#endif

            mesh.UploadMeshData(true);

            return mesh;
        }

        /// <summary>
        /// Creates a Mesh from polygon points
        /// </summary>
        /// <param name="poly">Polygon</param>
        /// <param name="path">Path from polygon</param>
        /// <param name="takeOffset">If offset must be subtracted from polygon point positions</param>
        /// <returns></returns>
        public static Mesh CreateMeshFromPolygon(PolygonCollider2D poly, int path, Color color, bool takeOffset)
        {
            Mesh mesh = new Mesh();

            Vector2 offsettosum = Vector2.zero;

            List<VertexTruple> baseTruple = new List<VertexTruple>();

            Vector3[] posarray;
            Vector2[] uvarray;
            Color[] colorarray;

            List<Vector2> polypathpoints = new List<Vector2>();

            if (takeOffset)
            {
                offsettosum = poly.offset;
            }

            poly.GetPath(path, polypathpoints);

            for (int i = 0; i < polypathpoints.Count; i++)
            {
                baseTruple.Add(new VertexTruple() { pos = polypathpoints[i] + offsettosum, col = color, uv = Vector2.zero });
            }

            posarray = VertexTruple.GetPosArray(baseTruple);
            uvarray = VertexTruple.GetUVArray(baseTruple);
            colorarray = VertexTruple.GetColorArray(baseTruple);
#if(false)
            Triangulator triangulator = new Triangulator(posarray);

            mesh.vertices = posarray;
            mesh.uv = uvarray;
            mesh.colors = colorarray;
            mesh.triangles = triangulator.Triangulate();

#endif

            mesh.UploadMeshData(true);

            return mesh;
        }



        /// <summary>
        /// Creates a Mesh from a point list (must be given CCW sorted)
        /// </summary>
        /// <param name="pointList">List of 3D points</param>
        /// <param name="uvList">UV List of UV coordinates</param>
        /// <param name="color">Color to apply to all vertexes</param>
        /// <param name="mesh">Previously cleared mesh to insert points</param>
        /// <param name="inverseTriangulation">If Triangulator should use CW instead (for CULLING)</param>
        /// <param name="fakedFlatPointList">If this list wants to be used for triangulation (real 3D point lists unfolded)</param>
        public static void FeedArraysForMeshFromPointList(ref Span<int> temp_triangulationV, ref GrowingStackArray<int> triangulation_result, ref GrowingStackArray<Vector3> sumposarray, ref GrowingStackArray<Vector2> sumuvarray, ref GrowingStackArray<Color> sumcolorarray, ref GrowingStackArray<int> sumtrianglearray, Span<Vector3> pointList, Span<Vector2> uvList, Color color, Span<Vector3> fakedFlatPointList, bool inverseTriangulation = false, bool fakedPresent = false)
        {
            SpecialTriangulatorStackArray triangulation_readpoints;

            if (fakedPresent)
            {
                triangulation_readpoints = new SpecialTriangulatorStackArray(fakedFlatPointList, false);
            }
            else
            {
                triangulation_readpoints = new SpecialTriangulatorStackArray(pointList, true);
            }


            /* Clear count of triangulation indexes result, so it can be used again */
            triangulation_result.Clear(false, 0);


            /* Triangulate over stack values */
            TriangulatorClass.TriangulateFast(ref triangulation_readpoints, ref triangulation_result, ref temp_triangulationV);

            /* Sum previous already existing position indexes, so this iteration triangles match this iteration new positions */
            int prevtriangles = sumposarray.Count;

            if (inverseTriangulation)
            {
                for (int i = triangulation_result.Count-1; i >= 0; i--)
                {
                    sumtrianglearray.Add(triangulation_result[i] + prevtriangles);
                }
            }
            else
            {
                for (int i = 0; i < triangulation_result.Count; i++)
                {
                    sumtrianglearray.Add(triangulation_result[i] + prevtriangles);
                }
            }


            for (int i=0;i<pointList.Length;i++)
            {

                sumposarray.Add(pointList[i]);
                sumuvarray.Add(uvList[i]);
                sumcolorarray.Add(color);
            }

            
            
        }
    }

}
