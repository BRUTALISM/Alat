﻿using UnityEngine;
using System.Collections;

namespace Alat
{
	public class RepeatedCubeVectorField : IVectorField
	{
		private Vector3[,,] gradient;
		
		public float Intensity { get; set; }
		public int Dimension { get; private set; }
		
		private const int DefaultDimension = 8;
		
		public RepeatedCubeVectorField(float intensity, int dimension = DefaultDimension)
		{
			Intensity = intensity;
			Dimension = dimension;

			gradient = new Vector3[dimension, dimension, dimension];
			for (int i = 0; i < dimension; i++)
			{
				for (int j = 0; j < dimension; j++)
				{
					for (int k = 0; k < dimension; k++)
					{
						gradient[i, j, k] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),
							Random.Range(-1f, 1f)).normalized;
					}
				}
			}
		}
		
		public Vector3 VectorAt(float x, float y, float z)
		{
			if (x < 0f) x = Mathf.Abs(x);
			if (y < 0f) y = Mathf.Abs(y);
			if (z < 0f) z = Mathf.Abs(z);
			
			int floorX = Mathf.FloorToInt(x);
			float tx = x - floorX;
			floorX %= Dimension;
			int ceilX = Mathf.CeilToInt(x) % Dimension;
			
			int floorY = Mathf.FloorToInt(y);
			float ty = y - floorY;
			floorY %= Dimension;
			int ceilY = Mathf.CeilToInt(y) % Dimension;
			
			int floorZ = Mathf.FloorToInt(z);
			float tz = z - floorZ;
			floorZ %= Dimension;
			int ceilZ = Mathf.CeilToInt(z) % Dimension;

			var omtx = 1f - tx;
			var omty = 1f - ty;
			var omtz = 1f - tz;
			
			return (tx * ty * tz * gradient[ceilX, ceilY, ceilZ] +
				tx * ty * omtz * gradient[ceilX, ceilY, floorZ] +
				tx * omty * tz * gradient[ceilX, floorY, ceilZ] +
				tx * omty * omtz * gradient[ceilX, floorY, floorZ] +
				omtx * ty * tz * gradient[floorX, ceilY, ceilZ] +
				omtx * ty * omtz * gradient[floorX, ceilY, floorZ] +
				omtx * omty * tz * gradient[floorX, floorY, ceilZ] +
				omtx * omty * omtz * gradient[floorX, floorY, floorZ]).normalized * Intensity;
		}
		
		public Vector3 VectorAt(Vector3 position)
		{
			return VectorAt(position.x, position.y, position.z);
		}
	}
}
