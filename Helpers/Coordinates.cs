using UnityEngine;
using System.Collections.Generic;

public static class Coordinates
{
	/// <summary>
	/// Converts spherical coordinates to cartesian ones. The angles are in degrees.
	/// </summary>
	public static Vector3 SphericalToCartesian(float radius, float polarAngle, float azimuthAngle)
	{
		var sinPolar = Mathf.Sin(polarAngle);
		var x = radius * Mathf.Cos(azimuthAngle) * sinPolar;
		var y = radius * Mathf.Sin(azimuthAngle) * sinPolar;
		var z = radius * Mathf.Cos(polarAngle);
		return new Vector3(x, y, z);
	}
}
