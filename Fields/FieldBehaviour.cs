using UnityEngine;
using System.Collections.Generic;
using Alat;

/// <summary>
/// An abstract class encapsulating a vector field inside a MonoBehaviour. Subclasses implement the underlying field.
/// </summary>
public abstract class FieldBehaviour : MonoBehaviour, IVectorField
{
	[System.Serializable]
	public class GizmoOptions
	{
		public bool DrawGizmos = false;
		public float GizmoExtents = 10f;
		public float GizmoSamplingStep = 1f;
		public Color GizmoColor = Color.white;
	}

	#region Editor public fields
	public GizmoOptions GizmoDrawingOptions;
	public float LookupScale = 1f;
	#endregion

	#region Abstract members
	public abstract IVectorField Field { get; }
	#endregion

	#region Public properties
	#endregion

	#region Private fields
	#endregion

	#region Unity methods

	void OnDrawGizmos()
	{
		if (Application.isPlaying && GizmoDrawingOptions.DrawGizmos)
		{
			Gizmos.color = GizmoDrawingOptions.GizmoColor;
			int totalSteps = Mathf.CeilToInt(GizmoDrawingOptions.GizmoExtents / GizmoDrawingOptions.GizmoSamplingStep);
			var samplingStartPosition = - GizmoDrawingOptions.GizmoExtents / 2;
			for (int i = 0; i < totalSteps; i++)
			{
				float x = samplingStartPosition + i * GizmoDrawingOptions.GizmoSamplingStep;
				for (int j = 0; j < totalSteps; j++)
				{
					float y = samplingStartPosition + j * GizmoDrawingOptions.GizmoSamplingStep;
					for (int k = 0; k < totalSteps; k++)
					{
						float z = samplingStartPosition + k * GizmoDrawingOptions.GizmoSamplingStep;
						var rootPosition = transform.position + new Vector3(x, y, z);
						var targetPosition = rootPosition + VectorAt(rootPosition);
						Draw.GizmoArrow(rootPosition, targetPosition);
					}
				}
			}
		}
	}

	#endregion

	#region IVectorField
	public Vector3 VectorAt(Vector3 worldPosition)
	{
		return Field.VectorAt(worldPosition * LookupScale);
	}
	#endregion
}
