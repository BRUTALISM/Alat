using UnityEngine;

namespace Alat
{
	public interface IVectorField
	{
		Vector3 VectorAt(Vector3 worldPosition);
	}
}
