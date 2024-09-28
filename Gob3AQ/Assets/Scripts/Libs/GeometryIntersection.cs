using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MVerse.Libs.GeometryIntersection
{
    public static class GeometryIntersection
    {
        public static bool CirclesIntersection(Vector3 pos1, float rad1, Vector3 pos2, float rad2, out Vector3[] intersectionPoints)
        {
            bool retVal = false;
            intersectionPoints = null;
            Vector3 deltaCenter = pos2 - pos1;
            float centerDistance = deltaCenter.magnitude;

            /* Deben alcanzarse, si estan muy lejos no se tocan */
            if ((centerDistance <= rad1 + rad2) && centerDistance >= Mathf.Abs(rad1 - rad2))
            {
                intersectionPoints = new Vector3[2];
                float sqrad1 = rad1 * rad1;
                float l = (sqrad1 - rad2 * rad2 + centerDistance * centerDistance) / (2 * centerDistance);
                float h = Mathf.Sqrt(sqrad1 - l * l);

                float ltod = l / centerDistance;
                float htod = h / centerDistance;

                float aux1x = ltod * deltaCenter.x + pos1.x;
                float aux2x = htod * deltaCenter.y;

                float aux1y = ltod * deltaCenter.y + pos1.y;
                float aux2y = htod * deltaCenter.x;


                intersectionPoints[0] = new Vector3(aux1x + aux2x, aux1y - aux2y);
                intersectionPoints[1] = new Vector3(aux1x - aux2x, aux1y + aux2y);

                retVal = true;

            }


            return retVal;
        }

        public static bool GetLineIntersection(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, out Vector2 intersection, bool avoidExtremePoints = true)
        {
            float s02_x, s02_y, s10_x, s10_y, s32_x, s32_y, s_numer, t_numer, denom, t;
            s10_x = p1.x - p0.x;
            s10_y = p1.y - p0.y;
            s32_x = p3.x - p2.x;
            s32_y = p3.y - p2.y;

            intersection = Vector2.zero;

            denom = s10_x * s32_y - s32_x * s10_y;
            if (denom == 0)
            {
                return false; // Collinear
            }

            bool denomPositive = denom > 0;

            s02_x = p0.x - p2.x;
            s02_y = p0.y - p2.y;
            s_numer = s10_x * s02_y - s10_y * s02_x;
            if ((s_numer < 0) == denomPositive)
                return false; // No collision

            t_numer = s32_x * s02_y - s32_y * s02_x;
            if ((t_numer < 0) == denomPositive)
                return false; // No collision

            if (((s_numer > denom) == denomPositive) || ((t_numer > denom) == denomPositive))
                return false; // No collision
                              // Collision detected
            t = t_numer / denom;

            /* Exact collision at the end of the line will belong to next line */
            if (avoidExtremePoints && ((t == 1) || (t == 0)))
            {
                return false;
            }

            intersection = new Vector2(p0.x + (t * s10_x), p0.y + (t * s10_y));

            return true;
        }

        public static bool IsInPolygon(Vector2[] polyPath, Vector2 p)
        {
            Vector2 p1, p2;
            bool inside = false;

            if (polyPath.Length < 3)
            {
                return inside;
            }

            Vector2 oldPoint = polyPath[polyPath.Length - 1];

            for (int i = 0; i < polyPath.Length; i++)
            {
                Vector2 newPoint = polyPath[i];

                if (newPoint.x > oldPoint.x)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if (((newPoint.x < p.x) == (p.x <= oldPoint.x)) && (((p.y - (long)p1.y) * (p2.x - p1.x)) < ((p2.y - (long)p1.y) * (p.x - p1.x))))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }

        public static bool IsInPolygon2(Vector2 point, Vector2[] path)
        {
            bool result = false;
            Vector2 a = path[path.Length - 1];
            foreach (Vector2 b in path)
            {
                if ((b.x == point.x) && (b.y == point.y))
                    return true;

                if ((b.y == a.y) && (point.y == a.y) && (a.x <= point.x) && (point.x <= b.x))
                    return true;

                if ((b.y < point.y) && (a.y >= point.y) || (a.y < point.y) && (b.y >= point.y))
                {
                    if (b.x + (point.y - b.y) / (a.y - b.y) * (a.x - b.y) <= point.x)
                        result = !result;
                }
                a = b;
            }
            return result;
        }

        public static bool IsInPolygon3(Vector2 point, Vector2[] polygon)
        {
            int polygonLength = polygon.Length, i = 0;
            bool inside = false;
            // x, y for tested point.
            float pointX = point.x, pointY = point.y;
            // start / end point for the current polygon segment.
            float startX, startY, endX, endY;
            Vector2 endPoint = polygon[polygonLength - 1];
            endX = endPoint.x;
            endY = endPoint.y;
            while (i < polygonLength)
            {
                startX = endX;
                startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.x;
                endY = endPoint.y;
                bool cond1;
                float leftcomp, rightcomp;

                /* Terminal points are not included */
                cond1 = (endY > pointY) ^ (startY > pointY);

                if (cond1)
                {
                    leftcomp = pointX - endX;
                    rightcomp = (pointY - endY) * (startX - endX) / (startY - endY);

                    /* Points touching line of polygon will be discarded */
                    if (leftcomp == rightcomp)
                    {
                        inside = false;
                        break;
                    }
                    else
                    {
                        inside ^= leftcomp < rightcomp;
                    }
                }
            }
            return inside;
        }


        public static bool IsInPolygon3_Original(Vector2 point, Vector2[] polygon)
        {
            int polygonLength = polygon.Length, i = 0;
            bool inside = false;
            // x, y for tested point.
            float pointX = point.x, pointY = point.y;
            // start / end point for the current polygon segment.
            float startX, startY, endX, endY;
            Vector2 endPoint = polygon[polygonLength - 1];
            endX = endPoint.x;
            endY = endPoint.y;
            while (i < polygonLength)
            {
                startX = endX;
                startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.x;
                endY = endPoint.y;


                inside ^= ((endY > pointY) ^ (startY > pointY)) /* ? pointY inside (startY;endY) segment ? */
                          && /* if so, test if it is under the segment */
                          ((pointX - endX) < ((pointY - endY) * (startX - endX) / (startY - endY)));

            }
            return inside;
        }


        public static bool LineSegmentsIntersectionWithPrecisonControl(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 intersection, float fSelfDefinedEpsilon = 1.0f)
        {
            //Debug.Log (string.Format("LineSegmentsIntersection2 p1 {0} p2 {1} p3 {2} p4{3}", p1, p2, p3, p4)); // the float value precision in the log is just 0.0f
            UnityEngine.Assertions.Assert.IsTrue(fSelfDefinedEpsilon > 0);

            float Ax, Bx, Cx, Ay, By, Cy, d, e, f, num/*,offset*/;
            float x1lo, x1hi, y1lo, y1hi;
            Ax = p2.x - p1.x;
            Bx = p3.x - p4.x;

            // X bound box test/
            if (Ax < 0)
            {
                x1lo = p2.x; x1hi = p1.x;
            }
            else
            {
                x1hi = p2.x; x1lo = p1.x;
            }

            if (Bx > 0)
            {
                if ((x1hi < p4.x && Mathf.Abs(x1hi - p4.x) > fSelfDefinedEpsilon)
                    || (p3.x < x1lo && Mathf.Abs(p3.x - x1lo) > fSelfDefinedEpsilon))
                    return false;
            }
            else
            {
                if ((x1hi < p3.x && Mathf.Abs(x1hi - p3.x) > fSelfDefinedEpsilon)
                    || (p4.x < x1lo && Mathf.Abs(p4.x - x1lo) > fSelfDefinedEpsilon))
                    return false;
            }

            Ay = p2.y - p1.y;
            By = p3.y - p4.y;

            // Y bound box test//
            if (Ay < 0)
            {
                y1lo = p2.y; y1hi = p1.y;
            }
            else
            {
                y1hi = p2.y; y1lo = p1.y;
            }

            if (By > 0)
            {
                if ((y1hi < p4.y && Mathf.Abs(y1hi - p4.y) > fSelfDefinedEpsilon)
                    || (p3.y < y1lo && Mathf.Abs(p3.y - y1lo) > fSelfDefinedEpsilon))
                    return false;
            }
            else
            {
                if ((y1hi < p3.y && Mathf.Abs(y1hi - p3.y) > fSelfDefinedEpsilon)
                    || (p4.y < y1lo && Mathf.Abs(p4.y - y1lo) > fSelfDefinedEpsilon))
                    return false;
            }

            Cx = p1.x - p3.x;
            Cy = p1.y - p3.y;
            d = By * Cx - Bx * Cy;  // alpha numerator//
            f = Ay * Bx - Ax * By;  // both denominator//

            // alpha tests//

            if (f > 0)
            {
                if ((d < 0 && Mathf.Abs(d) > fSelfDefinedEpsilon)
                    || (d > f && Mathf.Abs(d - f) > fSelfDefinedEpsilon))
                    return false;
            }
            else
            {
                if ((d > 0 && Mathf.Abs(d) > fSelfDefinedEpsilon)
                    || (d < f && Mathf.Abs(d - f) > fSelfDefinedEpsilon))
                    return false;
            }
            e = Ax * Cy - Ay * Cx;  // beta numerator//

            // beta tests //

            if (f > 0)
            {
                if ((e < 0 && Mathf.Abs(e) > fSelfDefinedEpsilon)
                    || (e > f) && Mathf.Abs(e - f) > fSelfDefinedEpsilon)
                    return false;
            }
            else
            {
                if ((e > 0 && Mathf.Abs(e) > fSelfDefinedEpsilon)
                    || (e < f && Mathf.Abs(e - f) > fSelfDefinedEpsilon))
                    return false;
            }

            // check if they are parallel
            if (f == 0 && Mathf.Abs(f) > fSelfDefinedEpsilon)
                return false;

            // compute intersection coordinates //
            num = d * Ax; // numerator //

            //    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;   // round direction //

            //    intersection.x = p1.x + (num+offset) / f;
            intersection.x = p1.x + num / f;
            num = d * Ay;

            //    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;

            //    intersection.y = p1.y + (num+offset) / f;
            intersection.y = p1.y + num / f;
            return true;
        }

        private static bool same_sign(float a, float b)
        {
            return ((a * b) >= 0f);
        }
    }
}

