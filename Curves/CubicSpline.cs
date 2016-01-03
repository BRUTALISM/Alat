using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// An immutable cubic spline data class.
/// </summary>
public class CubicSpline
{
	/// <summary>
	/// All the points comprising the spline. The data is layed out as [S1, C1, C2, S2, C3, C4, S3, ...], where Sn are
	/// spline points and Cn are control points.
	/// </summary>
	/// <value>The points.</value>
	public IEnumerable<Vector3> Points { get { return points; } }
	private List<Vector3> points;

	public CubicSpline(IEnumerable<Vector3> points)
	{
		if (points == null) throw new System.ArgumentException("points can't be null");
		if ((points.Count() - 1) % 3 != 0) throw new System.ArgumentException("points needs to have 1 + 3n elements");

		this.points = new List<Vector3>(points);
	}
}
