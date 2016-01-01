using UnityEngine;
using System.Collections.Generic;
using Alat;

public class RepeatedCubeVectorFieldBehaviour : FieldBehaviour
{
	#region Editor public fields
	public float Intensity = 1f;
	public int Dimension = 5;
	#endregion

	#region Public properties
	#endregion

	#region Private fields
	private RepeatedCubeVectorField field;
	#endregion

	#region FieldBehaviour
	public override IVectorField Field { get { return field; } }
	#endregion

	#region Unity methods

	void OnEnable()
	{
		field = new RepeatedCubeVectorField(Intensity, Dimension);
	}

	void OnDisable()
	{}

	#endregion
}
