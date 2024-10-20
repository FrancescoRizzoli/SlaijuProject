using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class Commons
    {
        public const uint defaultUintValue = 999;
        //conversion constant
        public const float DPI_2_PCM = 0.393701f;
        public const float CM_2_INCHES = 2.54f;

       /// <summary>
       /// scale the vector based on the dpi of the screen
       /// </summary>
       /// <param name="distance"></param>
        public static void ScaleByDPI(ref Vector2 distance)
        {
            float dpi = Screen.dpi;

            if (dpi <= 0.0f)
                dpi = 240.0f;

            float pcm = dpi * DPI_2_PCM;

            distance /= pcm;
        }

        public static float CmToPixels(float cm)
        {
            return cm / CM_2_INCHES * Screen.dpi;
        }
        
        public static bool InRange(this Vector2 me, Vector2 reference, float angleTolerance)
        {
            return Vector2.Dot(me.normalized, reference.normalized) >= Mathf.Cos(angleTolerance * Mathf.Deg2Rad);
            
        }

        /// <summary>
        /// get the angle between two vector 2 point in the screen
        /// </summary>
        /// <param name="StartPoint"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static float TwoPointAngle(Vector2 StartPoint, Vector2 endpoint)
        {
           return (float)(Math.Atan2((StartPoint.y - endpoint.y), (StartPoint.x - endpoint.x)) * (180 / Math.PI));

        }

        /// <summary>
        /// draw a debug box shape by the given parameters
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="rotation"></param>
        public static void DebugDrawOverlapBox(Vector3 position, Vector3 size, Quaternion rotation)
        {
            Color color = Color.yellow; // Color for visualization
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, size);
            DebugDrawBox(matrix, color, 2);
        }
        
        private static void DebugDrawBox(Matrix4x4 matrix, Color color, float time)
        {
            Vector3[] corners = new Vector3[8];
            corners[0] = matrix.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
            corners[1] = matrix.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
            corners[2] = matrix.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
            corners[3] = matrix.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
            corners[4] = matrix.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
            corners[5] = matrix.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));
            corners[6] = matrix.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));
            corners[7] = matrix.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));

            Debug.DrawLine(corners[0], corners[1], color, time);
            Debug.DrawLine(corners[1], corners[2], color, time);
            Debug.DrawLine(corners[2], corners[3], color, time);
            Debug.DrawLine(corners[3], corners[0], color, time);

            Debug.DrawLine(corners[4], corners[5], color, time);
            Debug.DrawLine(corners[5], corners[6], color, time);
            Debug.DrawLine(corners[6], corners[7], color, time);
            Debug.DrawLine(corners[7], corners[4], color, time);

            Debug.DrawLine(corners[0], corners[4], color, time);
            Debug.DrawLine(corners[1], corners[5], color, time);
            Debug.DrawLine(corners[2], corners[6], color, time);
            Debug.DrawLine(corners[3], corners[7], color, time);
        }

        public static Bounds TotalBounds(GameObject gameObject)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            bool firstBoundsInitialized = false;
            Bounds result = new Bounds();

            foreach (Renderer renderer in renderers)
            {
                if (renderer.gameObject.CompareTag("Particle"))
                    continue;

                if (!firstBoundsInitialized)
                {
                    result = renderer.bounds;
                    firstBoundsInitialized = true;
                }
                else
                {
                    result.Encapsulate(renderer.bounds);
                }
            }

            return result;
        }
        /// <summary>
        /// use the knuth shuffle to randomize a generic list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void KnuthShuffle<T>(List<T> list)
        {
            T tmp = list[0];
            for(int i = 0;i< list.Count; i++)
            {
                tmp = list[i];
                int random = UnityEngine.Random.Range(i, list.Count);
                list[i] = list[random];
                list[random] = tmp;
            }

        }
        /// <summary>
        /// use the knuth shuffle to randomize a generic array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void KnuthShuffle<T>(T[] array)
        {
            T tmp = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                tmp = array[i];
                int random = UnityEngine.Random.Range(i, array.Length);
                array[i] = array[random];
                array[random] = tmp;
            }

        }
        /// <summary>
        /// return the current date in int
        /// </summary>
        /// <returns></returns>
        public static int ConvertInvoiceDate()
        {
            return int.Parse(DateTime.Today.ToString("yyyyMMdd"));
        }

        public  static int LeaderboardValueIn(int integerValue, float floatValue)
        {
           
            float adjustedFloat = floatValue;
            while (adjustedFloat >= 1)
            {
                adjustedFloat /= 10;
            }

            
            float result = integerValue + adjustedFloat;

            result *= 10000;

            
            return Mathf.RoundToInt(result); 
        }
        public static int LeaderboardValueOut(int integerValue)
        {
            float result = integerValue;
            result = integerValue / 10000;
            return Mathf.RoundToInt(result);

        }

    }
}
