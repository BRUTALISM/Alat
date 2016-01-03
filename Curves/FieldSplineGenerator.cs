using UnityEngine;
using System.Collections.Generic;
using Alat;

public class FieldSplineGenerator
{
	[System.Serializable]
	public struct Parameters
	{
		public int NumberOfHops;
		[Range(0f, 1f)] public float ControlPointForwardCurvature;
		[Range(0f, 1f)] public float ControlPointBackwardCurvature;
	}

	private Parameters parameters;

	public FieldSplineGenerator(Parameters parameters)
	{
		this.parameters = parameters;
	}

	public CubicSpline FromField(IVectorField field, Vector3 startingPoint)
	{
		var points = new List<Vector3>(1 + parameters.NumberOfHops * 3);

		var currentPoint = startingPoint;
		points.Add(currentPoint);
		for (int i = 0; i < parameters.NumberOfHops; i++)
		{
			points.Add(currentPoint + parameters.ControlPointForwardCurvature * field.VectorAt(currentPoint));

			var halfwayPoint = currentPoint + 0.5f * field.VectorAt(currentPoint);
			currentPoint = halfwayPoint + 0.5f * field.VectorAt(halfwayPoint);
			points.Add(currentPoint - parameters.ControlPointBackwardCurvature * field.VectorAt(currentPoint));
			points.Add(currentPoint);
		}

		return new CubicSpline(points);
	}
}
