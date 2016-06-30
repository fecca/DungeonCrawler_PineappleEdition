using System.Collections.Generic;
using UnityEngine;

public class RoomFactory
{
	private static RoomFactory _instance;
	public static RoomFactory Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new RoomFactory();
			}
			return _instance;
		}
	}

	private RoomFactory() { }

	public GameObject CreateRoom(Vector3 position, int numberOfCorners, int radius, int thickness, int height)
	{
		var vertices = new List<Vector3>();
		var triangles = new List<int>();
		var floorVertices = new Vector3[numberOfCorners + 1];

		// Origin
		vertices.Add(position);

		GenerateFloorVertices(position, numberOfCorners, radius, ref vertices, ref floorVertices);
		SetTriangleIndices(floorVertices.Length, ref triangles);

		return CompleteGameObject(vertices, triangles);
	}

	private void GenerateFloorVertices(Vector3 position, int numberOfCorners, int radius, ref List<Vector3> vertices, ref Vector3[] floorVertices)
	{
		var angle = Random.Range(0f, 360f);
		var angleStep = 360f / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var modifiedAngle = angle + (angleStep * 0.5f) * Random.value;
			var modifiedRadius = radius * Random.Range(0.8f, 1.2f);
			var x = Mathf.Sin(Mathf.Deg2Rad * modifiedAngle) * modifiedRadius;
			var z = Mathf.Cos(Mathf.Deg2Rad * modifiedAngle) * modifiedRadius;
			var vertexPosition = new Vector3(x, position.y, z);
			vertexPosition += position;
			floorVertices[i] = vertexPosition;
			vertices.Add(vertexPosition);
			angle += 360f / numberOfCorners;
		}
	}
	private void SetTriangleIndices(int numberOfFloorVertices, ref List<int> triangles)
	{
		for (var i = 1; i <= numberOfFloorVertices; i++)
		{
			var thisIndex = i < numberOfFloorVertices ? i : 1;
			var nextIndex = thisIndex < numberOfFloorVertices - 1 ? thisIndex + 1 : 1;
			triangles.Add(0);
			triangles.Add(thisIndex);
			triangles.Add(nextIndex);
		}
	}
	private GameObject CompleteGameObject(List<Vector3> vertices, List<int> triangles)
	{
		var newGameObject = new GameObject();
		var meshFilter = newGameObject.AddComponent<MeshFilter>();
		var meshRenderer = newGameObject.AddComponent<MeshRenderer>();
		var mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		meshFilter.mesh = mesh;
		meshRenderer.material = Resources.Load<Material>("WallMaterial_Unlit");

		return newGameObject;
	}
}