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

		GenerateVertices(position, numberOfCorners, radius, thickness, ref vertices);
		GenerateTriangles(numberOfCorners, vertices.Count, ref triangles);

		return CompleteGameObject(vertices, triangles);
	}

	private void GenerateVertices(Vector3 position, int numberOfCorners, int radius, int thickness, ref List<Vector3> vertices)
	{
		// Origin
		vertices.Add(position);

		var angle = Random.Range(0f, 360f);
		var angleStep = 360f / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var modifiedAngle = angle + (angleStep * 0.5f) * Random.value;
			var modifiedRadius = radius * Random.Range(0.8f, 1.2f);
			var x = Mathf.Sin(Mathf.Deg2Rad * modifiedAngle) * modifiedRadius;
			var z = Mathf.Cos(Mathf.Deg2Rad * modifiedAngle) * modifiedRadius;
			var floorVertexPosition = new Vector3(x, position.y, z);
			floorVertexPosition += position;

			var heading = floorVertexPosition - position;
			var distance = heading.magnitude;
			var direction = heading / distance;
			var floorThicknessVertexPosition = floorVertexPosition + heading.normalized;

			vertices.Add(floorVertexPosition);
			vertices.Add(floorThicknessVertexPosition);

			angle += 360f / numberOfCorners;
		}
	}
	private void GenerateTriangles(int numberOfCorners, int numberOfVertices, ref List<int> triangles)
	{
		var groupStep = (numberOfVertices - 1) / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var thisIndex = i;
			var thisGroup = (groupStep * thisIndex);
			var thisCornerVertex = thisGroup + 0;
			var thisCornerThicknessVertex = thisGroup + 1;

			var nextIndex = (i < numberOfCorners - 1 ? i + 1 : 0);
			var nextGroup = (groupStep * nextIndex);
			var nextCornerVertex = nextGroup + 0;
			var nextCornerThicknessVertex = nextGroup + 1;

			// Floor
			triangles.Add(0);
			triangles.Add(thisCornerVertex + 1);
			triangles.Add(nextCornerVertex + 1);

			// Thickness
			triangles.Add(thisCornerVertex + 1);
			triangles.Add(thisCornerThicknessVertex + 1);
			triangles.Add(nextCornerVertex + 1);

			triangles.Add(thisCornerThicknessVertex + 1);
			triangles.Add(nextCornerThicknessVertex + 1);
			triangles.Add(nextCornerVertex + 1);

			Debug.Log("#asdf# " + thisIndex + ", " + thisGroup + ", " + thisCornerVertex + ", " + thisCornerThicknessVertex);
			Debug.Log("#asdf# " + nextIndex + ", " + nextGroup + ", " + nextCornerVertex + ", " + nextCornerThicknessVertex);
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