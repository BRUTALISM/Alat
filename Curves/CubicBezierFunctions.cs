using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class CubicBezierFunctions
{
	/// <summary>
	/// Gets the interpolated point from a given collection of points. Points at indices 0 and 3 are treated as points
	/// on the curve, while the ones at 1 and 2 are treated as control points.
	/// </summary>
	public static Vector3 InterpolatePoint(IEnumerable<Vector3> points, float t)
	{
		float omt = 1f - t;
		float omt2 = omt * omt;
		float t2 = t * t;
		return points.ElementAt(0) * (omt2 * omt) +
			points.ElementAt(1) * (3f * omt2 * t) +
			points.ElementAt(2) * (3f * omt * t2) +
			points.ElementAt(3) * (t2 * t);
	}

	/// <summary>
	/// Gets the entire spline segment as a collection of interpolated points. <paramref name="points"/> at indices 0
	/// and 3 are treated as points on the curve, while the ones at 1 and 2 are treated as control points. The number of
	/// interpolated points is controlled by the <paramref name="subdivisions"/> parameter.
	/// </summary>
	public static IEnumerable<Vector3> InterpolatePoints(IEnumerable<Vector3> points, int subdivisions)
	{
		var interpolatedPoints = new List<Vector3>(subdivisions);
		for (int i = 0; i <= subdivisions; i++)
		{
			var t = ((float)i) / subdivisions;
			interpolatedPoints.Add(InterpolatePoint(points, t));
		}
		return interpolatedPoints;
	}
}
