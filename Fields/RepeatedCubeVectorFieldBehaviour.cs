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
	public override IVectorField Field
	{
		get
		{
			if (field == null) GenerateField();
			return field;
		}
	}
	#endregion

	#region Unity methods

	void OnEnable()
	{
		GenerateField();
	}

	void OnDisable()
	{
		field = null;
	}

	#endregion

	#region Field generation

	private void GenerateField()
	{
		field = new RepeatedCubeVectorField(Intensity, Dimension);	
	}

	#endregion
}
