using UnityEngine;
using System.Collections.Generic;

namespace Alat
{
	public static class Numbers
	{
		#region Float extensions
		
		/// <summary>
		/// Returns the fractional part of the given float (the part after the decimal point). For negative numbers, it
		/// returns a positive number calculated by subtracting the fractional part from 1.
		/// </summary>
		public static float PositiveFract(this float self)
		{
			return self - Mathf.Floor(self);
		}
		
		/// <summary>
		/// Wraps the given value so that it's always in the [0f, 1f] range.
		/// </summary>
		public static float Wrap01(this float self)
		{
			return self >= 0f && self <= 1f ? self : self - Mathf.Floor(self);
		}
		
		#endregion
		
		#region Integer extensions
		
		public static void Times(this int self, System.Action<int> loopBody)
		{
			if (loopBody != null) for (int i = 0; i < self; i++) loopBody(i);
		}

		public static IEnumerable<TResult> Times<TResult>(this int self, System.Func<int, TResult> producer)
		{
			var result = new List<TResult>(self);
			for (int i = 0; i < self; i++) result.Add(producer(i));
			return result;
		}
		
		#endregion
	}
}
