using UnityEngine;
using System.Collections;

namespace Alat
{
	public static class Vectors
	{
		private const float VectorAngleToleranceDegrees = 0.2f;
		
		public static Vector3 RandomUnitVector3()
		{
			return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
		}
		
		public static Vector3 RandomFlatVector3()
		{
			var vector = RandomUnitVector3();
			vector.y = 0f;
			return vector;
		}

		public static Vector3 RandomSpherical()
		{
			float polarAngle = Random.Range(0f, Mathf.Rad2Deg * (2 * Mathf.PI));
			float azimuthAngle = Mathf.Rad2Deg * Mathf.Acos(Random.Range(-1f, 1f));
			return Coordinates.SphericalToCartesian(1f, polarAngle, azimuthAngle);
		}
		
		public static Vector2 RandomRotatedUnitVector2()
		{
			return (Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) * Vector3.forward).XZ();
		}
		
		public static Vector3 RandomRotation(this Vector3 self, Vector3 axis, float maxAngleDegrees)
		{
			return Quaternion.AngleAxis(Random.Range(-maxAngleDegrees / 2, maxAngleDegrees / 2), axis) * self;
		}
		
		public static Vector2 XZ(this Vector3 self) { return new Vector2(self.x, self.z); }
		
		public static Vector3 ToXZ(this Vector2 self) { return new Vector3(self.x, 0f, self.y); }
		
		public static bool IsBetween(this Vector2 self, Vector2 v1, Vector2 v2)
		{
			return Vector2.Angle(v1, self) < VectorAngleToleranceDegrees ||
				Vector2.Angle(self, v2) < VectorAngleToleranceDegrees ||
				Mathf.Sign(Vector3.Cross(v1.ToXZ(), self.ToXZ()).y) ==
				Mathf.Sign(Vector3.Cross(self.ToXZ(), v2.ToXZ()).y);
		}
		
		public static bool IsBetween(this Vector3 self, Vector3 v1, Vector3 v2)
		{
			return Vector3.Angle(v1, self) < VectorAngleToleranceDegrees ||
				Vector3.Angle(self, v2) < VectorAngleToleranceDegrees ||
				Mathf.Sign(Vector3.Cross(v1, self).y) == Mathf.Sign(Vector3.Cross(self, v2).y);
		}
		
		public static Vector3 RotateAround(this Vector3 self, Vector3 pivot, Quaternion rotation)
		{
			return rotation * (self - pivot) + pivot;
		}
		
		public static Vector3 RoundComponents(this Vector3 self, float rounding)
		{
			return new Vector3(Mathf.Round(self.x / rounding) * rounding, Mathf.Round(self.y / rounding) * rounding,
				Mathf.Round(self.z / rounding) * rounding);
		}
		
		public static Vector3 CeilComponents(this Vector3 self, float rounding)
		{
			return new Vector3(Mathf.Ceil(self.x / rounding) * rounding, Mathf.Ceil(self.y / rounding) * rounding,
				Mathf.Ceil(self.z / rounding) * rounding);
		}
		
		public static Vector3 FloorComponents(this Vector3 self, float rounding)
		{
			return new Vector3(Mathf.Floor(self.x / rounding) * rounding, Mathf.Floor(self.y / rounding) * rounding,
				Mathf.Floor(self.z / rounding) * rounding);
		}
		
		public static Vector3 Abs(this Vector3 self)
		{
			return new Vector3(Mathf.Abs(self.x), Mathf.Abs(self.y), Mathf.Abs(self.z));
		}
		
		/// <summary>
		///     p1
		///      \   q1
		///       \ /
		///        X
		///       / \
		///      /   p2
		///     /
		///    q2
		/// </summary>
		public static Vector2? LineSegementsIntersect(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
		{
			// Found on http://www.codeproject.com/Tips/862988/Find-the-Intersection-Point-of-Two-Line-Segments
			
			const float Zero = 0.001f;
			
			System.Func<Vector2, Vector2, float> Cross = (v1, v2) =>
			{
				return v1.x * v2.y - v1.y * v2.x;
			};
			
			var r = p2 - p1;
			var s = q2 - q1;
			var rxs = Cross(r, s);
			var qpxr = Cross(q1 - p1, r);
			
			// If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
			if (Mathf.Abs(rxs) < Zero && Mathf.Abs(qpxr) < Zero) return null;
			
			// If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
			if (Mathf.Abs(rxs) < Zero && Mathf.Abs(qpxr) > Zero) return null;
			
			// t = (q - p) x s / (r x s)
			var t = Cross(q1 - p1, s) / rxs;
			
			// u = (q - p) x r / (r x s)
			var u = Cross(q1 - p1, r) / rxs;
			
			// If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1 the two line segments meet at the point p + t r = q + u s.
			if (Mathf.Abs(rxs) > Zero && (0 <= t && t <= 1) && (0 <= u && u <= 1))
			{
				// We can calculate the intersection point using either t or u.
				return p1 + t * r;
			}
			
			// Otherwise, the two line segments are not parallel but do not intersect.
			return null;
		}
	}
}
