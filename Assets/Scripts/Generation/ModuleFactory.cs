using System.Collections.Generic;
using UnityEngine;

public class ModuleFactory
{
	protected void CompleteGameObject(List<Vector3> vertices, List<int> triangles)
	{
		var newGameObject = new GameObject();
		newGameObject.transform.position = Vector3.zero;

		var mesh = new Mesh
		{
			vertices = vertices.ToArray(),
			triangles = triangles.ToArray()
		};
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		var meshFilter = newGameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		var meshRenderer = newGameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = Resources.Load<Material>("WallMaterial_Unlit");

		var meshCollider = newGameObject.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = mesh;
	}
}