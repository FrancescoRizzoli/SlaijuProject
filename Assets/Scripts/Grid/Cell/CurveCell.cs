using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Grid
{
	public class CurveCell : BaseCell
	{
		[SerializeField] private Transform[] curvePoint = Array.Empty<Transform>();

        private const float COSINE_TOLERANCE = 0.71f; // approximation of cos(45�) = 0.707

        public Transform GetExitPosition(Vector3 entranceDirection)
		{
			Vector3 otherEntrance = Vector3.zero;
			foreach (Vector3 v in safeSide)
				if (v != entranceDirection)
					otherEntrance = v;

			foreach (Transform t in curvePoint)
				if (Vector3.Dot(t.position - transform.position, otherEntrance) >= COSINE_TOLERANCE)
					return t;

			return null;
        }
		public Vector3 GetExitDirection(Vector3 characterForwardEntrance)
		{
			
			foreach (Vector3 t in safeSide)
			{
				if (characterForwardEntrance != t)
                    return t;

			}
                
			return Vector3.zero;
        }
	}
}
