using UnityEngine;
using System.Collections.Generic;

namespace Alat
{
	/// <summary>
	/// Helper class for producing Unity's Mesh instances procedurally.
	/// </summary>
	public class MeshBuilder
	{
		#region Public data
		
		public List<Vector3> Vertices { get; private set; }
		public List<int> Indices { get; private set; }
		public List<Vector3> Normals { get; private set; }
		public List<Color> Colors { get; private set; }
		public List<Vector4> Tangents { get; private set; }
		public List<Vector2> UVs { get; private set; }
		
		public IEnumerable<PartialMesh> PartialMeshes { get { return partials; } }
		
		#endregion
		
		#region Private data
		
		private List<PartialMesh> partials = new List<PartialMesh>();
		
		#endregion
		
		#region Constructor
		
		public MeshBuilder()
		{
			Vertices = new List<Vector3>();
			Indices = new List<int>();
			Normals = new List<Vector3>();
			Colors = new List<Color>();
			Tangents = new List<Vector4>();
			UVs = new List<Vector2>();
		}
		
		#endregion
		
		#region Building & clearing
		
		/// <summary>
		/// Builds meshes based on geometry packed so far.
		/// </summary>
		public List<Mesh> Build()
		{
			// Build a partial mesh for any leftover verts, indices, etc.
			PreparePartials();
			
			return PackPartials();
		}
		
		/// <summary>
		/// Transfers any remaining vertices, indices, normals, and colors into a new PartialMesh that is appended to
		/// the end of <c>PartialMeshes</c>.
		/// </summary>
		public void PreparePartials()
		{
			if (Vertices.Count > 0)
			{
				var partialMesh = new PartialMesh();
				
				partialMesh.Vertices = Vertices;
				partialMesh.Indices = Indices;
				partialMesh.Normals = Normals;
				partialMesh.Colors = Colors;
				partialMesh.Tangents = Tangents;
				partialMesh.UVs = UVs;
				
				partials.Add(partialMesh);
			}
			
			Clear();
		}
		
		/// <summary>
		/// Wraps mesh builder's geometry arrays into a new PartialMesh instance. Does not take into account any
		/// previously built partial meshes, nor does it clear mesh builder's internal lists. Use this only in cases
		/// where you need a local mesh builder to pack you some geometry and then use the produced partial mesh to pack
		/// it into some other mesh builder.
		/// </summary>
		/// <returns>The partial.</returns>
		public PartialMesh BuildPartial()
		{
			// TODO: Add splitting support for large partial meshes (>64k verts).
			
			PartialMesh partialMesh = new PartialMesh();
			partialMesh.Vertices = new List<Vector3>(Vertices);
			partialMesh.Indices = new List<int>(Indices);
			partialMesh.Normals = new List<Vector3>(Normals);
			partialMesh.Colors = new List<Color>(Colors);
			partialMesh.Tangents = new List<Vector4>(Tangents);
			partialMesh.UVs = new List<Vector2>(UVs);
			
			return partialMesh;
		}
		
		/// <summary>
		/// Clears mesh builder's internal geometry lists. Does not clear partial meshes.
		/// </summary>
		public void Clear()
		{
			if (Vertices.Count > 0) Vertices = new List<Vector3>();
			if (Indices.Count > 0) Indices = new List<int>();
			if (Normals.Count > 0) Normals = new List<Vector3>();
			if (Colors.Count > 0) Colors = new List<Color>();
			if (Tangents.Count > 0) Tangents = new List<Vector4>();
			if (UVs.Count > 0) UVs = new List<Vector2>();
		}
		
		#endregion
		
		#region Packing methods
		
		/// <summary>
		/// Pack the specified vertices and indices into this builder. The indices are expected to be relative to the
		/// <paramref name="vertices"/> array, not the already packed stuff in the builder (otherwise there'd be no
		/// point in having this class).
		/// </summary>
		public void Pack(List<Vector3> vertices, List<int> indices, List<Vector3> normals = null,
			List<Color> colors = null, List<Vector4> tangents = null, List<Vector2> uvs = null)
		{
			if (vertices.Count > 65534) throw new System.ArgumentException("vertices.Count must be less than 65534");
			if (this.Vertices.Count + vertices.Count > 65534) PreparePartials();
			
			// Copy indices
			int indexStart = this.Vertices.Count;
			foreach (int index in indices) this.Indices.Add(indexStart + index);
			
			// Copy vertices
			this.Vertices.AddRange(vertices);
			
			// Copy colors
			if (colors != null) this.Colors.AddRange(colors);
			
			// Copy normals
			if (normals != null) this.Normals.AddRange(normals);
			
			if (tangents != null) this.Tangents.AddRange(tangents);
			
			if (uvs != null) this.UVs.AddRange(uvs);
		}
		
		/// <summary>
		/// Pack the specified partialMesh into the builder. See the overloaded Pack method for details.
		/// </summary>
		public void Pack(PartialMesh partialMesh)
		{
			Pack(partialMesh.Vertices, partialMesh.Indices, partialMesh.Normals, partialMesh.Colors,
				partialMesh.Tangents, partialMesh.UVs);
		}
		
		#endregion
		
		#region Private methods
		
		private List<Mesh> PackPartials()
		{
			var meshes = new List<Mesh>();
			foreach (var partialMesh in partials)
			{
				Mesh mesh = new Mesh();
				
				mesh.vertices = partialMesh.Vertices.ToArray();
				mesh.triangles = partialMesh.Indices.ToArray();
				
				if (partialMesh.Normals != null && partialMesh.Normals.Count > 0)
				{
					mesh.normals = partialMesh.Normals.ToArray();
				}
				if (partialMesh.Colors != null && partialMesh.Colors.Count > 0)
				{
					mesh.colors = partialMesh.Colors.ToArray();
				}
				if (partialMesh.Tangents != null && partialMesh.Tangents.Count > 0)
				{
					mesh.tangents = partialMesh.Tangents.ToArray();
				}
				if (partialMesh.UVs != null && partialMesh.UVs.Count > 0)
				{
					mesh.uv = partialMesh.UVs.ToArray();
				}
				
				meshes.Add(mesh);
			}
			
			return meshes;
		}
		
		#endregion
	}
}