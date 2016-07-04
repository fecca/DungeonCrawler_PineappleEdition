using System.Collections.Generic;
using UnityEngine;

public class ModuleFactory
{
	protected const float AngleModifierIntervalMin = -0.5f;
	protected const float AngleModifierIntervalMax = 0.5f;
	protected const float RadiusModifierIntervalMin = 0.9f;
	protected const float RadiusModifierIntervalMax = 1.1f;
	protected const float FloorModifierIntervalMin = -0.1f;
	protected const float FloorModifierIntervalMax = 0.1f;
	protected const float WallModifierIntervalMin = -0.5f;
	protected const float WallModifierIntervalMax = 0.5f;
	protected const float CorridorModifierIntervalMin = 0.75f;
	protected const float CorridorModifierIntervalMax = 1.25f;

	protected GameObject CompleteGameObject(Vector3 position, List<Vector3> vertices, List<int> triangles)
	{
		var newGameObject = new GameObject("Module");
		newGameObject.transform.position = position;

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

		return newGameObject;
	}
}