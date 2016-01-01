using UnityEngine;
using System.Collections.Generic;
using Alat;

/// <summary>
/// An abstract class encapsulating a vector field inside a MonoBehaviour. Subclasses implement the underlying field.
/// </summary>
public abstract class FieldBehaviour : MonoBehaviour, IVectorField
{
	#region Editor public fields
	public bool DrawGizmos = false;
	public float GizmoExtents = 10f;
	public float GizmoSamplingStep = 1f;
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
		if (Application.isPlaying && DrawGizmos)
		{
			Gizmos.color = Color.green;
			
			int totalSteps = Mathf.CeilToInt(GizmoExtents / GizmoSamplingStep);
			var samplingStartPosition = -GizmoExtents / 2;
			for (int i = 0; i < totalSteps; i++)
			{
				float x = samplingStartPosition + i * GizmoSamplingStep;
				for (int j = 0; j < totalSteps; j++)
				{
					float y = samplingStartPosition + j * GizmoSamplingStep;
					for (int k = 0; k < totalSteps; k++)
					{
						float z = samplingStartPosition + k * GizmoSamplingStep;
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
		return Field.VectorAt(worldPosition);
	}
	#endregion
}
