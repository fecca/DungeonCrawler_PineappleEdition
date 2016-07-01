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

	private const float AngleModifierIntervalMin = -0.5f;
	private const float AngleModifierIntervalMax = 0.5f;
	private const float RadiusModifierIntervalMin = 0.9f;
	private const float RadiusModifierIntervalMax = 1.1f;
	private const float FloorModifierIntervalMin = -0.1f;
	private const float FloorModifierIntervalMax = 0.1f;
	private const float WallModifierIntervalMin = -0.5f;
	private const float WallModifierIntervalMax = 0.5f;

	public GameObject CreateRoom(Vector3 position, int numberOfCorners, int radius, int thickness, int height)
	{
		var vertices = GenerateVertices(position, numberOfCorners, radius, thickness, height);
		var triangles = GenerateTriangles(numberOfCorners, ref vertices);

		return CompleteGameObject(vertices, triangles);
	}
	public GameObject CreateUniqueVertexCube()
	{
		var vertices = new List<Vector3>();
		var triangles = new List<int>();

		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(0, 0, 1));
		vertices.Add(new Vector3(1, 0, 1));
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);

		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(1, 0, 1));
		vertices.Add(new Vector3(1, 0, 0));
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);

		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(1, 0, 0));
		vertices.Add(new Vector3(1, -1, 0));
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);

		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(1, -1, 0));
		vertices.Add(new Vector3(0, -1, 0));
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);

		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(0, -1, 0));
		vertices.Add(new Vector3(0, -1, 1));
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);

		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(0, -1, 1));
		vertices.Add(new Vector3(0, 0, 1));
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);

		return CompleteGameObject(vertices, triangles);
	}
	public GameObject CreateSharedVertexCube()
	{
		var vertices = new List<Vector3>();
		var triangles = new List<int>();

		vertices.Add(new Vector3(0, 0, 0));
		vertices.Add(new Vector3(0, 0, 1));
		vertices.Add(new Vector3(1, 0, 1));
		vertices.Add(new Vector3(1, 0, 0));
		vertices.Add(new Vector3(1, -1, 0));
		vertices.Add(new Vector3(0, -1, 0));
		vertices.Add(new Vector3(0, -1, 1));

		triangles.Add(0);
		triangles.Add(1);
		triangles.Add(2);

		triangles.Add(0);
		triangles.Add(2);
		triangles.Add(3);

		triangles.Add(0);
		triangles.Add(3);
		triangles.Add(4);

		triangles.Add(0);
		triangles.Add(4);
		triangles.Add(5);

		triangles.Add(0);
		triangles.Add(5);
		triangles.Add(6);

		triangles.Add(0);
		triangles.Add(6);
		triangles.Add(1);

		return CompleteGameObject(vertices, triangles);
	}

	private List<Vector3> GenerateVertices(Vector3 position, int numberOfCorners, int radius, int thickness, int height)
	{
		var vertices = new List<Vector3>();
		var angle = Random.Range(0f, 360f);
		var angleStep = 360f / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var modifiedAngle = angle + (angleStep * 0.5f) * Random.Range(AngleModifierIntervalMin, AngleModifierIntervalMax);
			var modifiedRadius = radius * Random.Range(RadiusModifierIntervalMin, RadiusModifierIntervalMax);
			var x = Mathf.Sin(Mathf.Deg2Rad * modifiedAngle) * modifiedRadius;
			var z = Mathf.Cos(Mathf.Deg2Rad * modifiedAngle) * modifiedRadius;

			// Origo
			vertices.Add(position);

			// Floor vertex 1
			var floorVertex1 = (new Vector3(x * 0.33f, position.y + Random.Range(FloorModifierIntervalMin, FloorModifierIntervalMax), z * 0.33f) + position);
			vertices.Add(floorVertex1);

			// Floor vertex 2
			var floorVertex2 = (new Vector3(x * 0.66f, position.y + Random.Range(FloorModifierIntervalMin, FloorModifierIntervalMax), z * 0.66f) + position);
			vertices.Add(floorVertex2);

			// Corner vertex
			var cornerVertex = new Vector3(x, position.y, z) + position;
			vertices.Add(cornerVertex);

			var directionNormalized = (cornerVertex - position).normalized;

			// Wall vertex
			var wallVertex = cornerVertex + (directionNormalized * Random.Range(WallModifierIntervalMin, WallModifierIntervalMax)) + (Vector3.up * height);
			vertices.Add(wallVertex);

			// Extended wall vertex
			var extendedWallVertex = wallVertex + directionNormalized + (directionNormalized * Random.Range(WallModifierIntervalMin, WallModifierIntervalMax));
			vertices.Add(extendedWallVertex);

			// Extended corner vertex
			var extendedCornerVertex = directionNormalized + cornerVertex;
			vertices.Add(extendedCornerVertex);

			angle += 360f / numberOfCorners;
		}

		return vertices;
	}
	private List<int> GenerateTriangles(int numberOfCorners, ref List<Vector3> vertices)
	{
		var newVertices = new List<Vector3>();
		var triangles = new List<int>();
		var numberOfVerticesPerGroup = vertices.Count / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var thisGroup = (numberOfVerticesPerGroup * i);
			var nextGroup = thisGroup + numberOfVerticesPerGroup >= vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

			var thisOrigoVertex = thisGroup;
			var thisFloorVertex1 = thisGroup + 1;
			var thisFloorVertex2 = thisGroup + 2;
			var thisCornerVertex = thisGroup + 3;
			var thisWallVertex = thisGroup + 4;
			var thisExtendedWallVertex = thisGroup + 5;
			var thisExtendedCornerVertex = thisGroup + 6;

			var nextFloorVertex1 = nextGroup + 1;
			var nextFloorVertex2 = nextGroup + 2;
			var nextCornerVertex = nextGroup + 3;
			var nextWallVertex = nextGroup + 4;
			var nextExtendedWallVertex = nextGroup + 5;
			var nextExtendedCornerVertex = nextGroup + 6;

			newVertices.Add(vertices[thisOrigoVertex]);
			newVertices.Add(vertices[thisFloorVertex1]);
			newVertices.Add(vertices[nextFloorVertex1]);
			triangles.Add(newVertices.Count - 3);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 1);

			newVertices.Add(vertices[thisFloorVertex1]);
			newVertices.Add(vertices[thisFloorVertex2]);
			newVertices.Add(vertices[nextFloorVertex2]);
			newVertices.Add(vertices[nextFloorVertex1]);
			triangles.Add(newVertices.Count - 4);
			triangles.Add(newVertices.Count - 3);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 1);
			triangles.Add(newVertices.Count - 4);

			newVertices.Add(vertices[thisCornerVertex]);
			newVertices.Add(vertices[nextCornerVertex]);
			newVertices.Add(vertices[nextFloorVertex2]);
			newVertices.Add(vertices[thisFloorVertex2]);
			triangles.Add(newVertices.Count - 4);
			triangles.Add(newVertices.Count - 3);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 1);
			triangles.Add(newVertices.Count - 4);

			newVertices.Add(vertices[thisCornerVertex]);
			newVertices.Add(vertices[thisWallVertex]);
			newVertices.Add(vertices[nextWallVertex]);
			newVertices.Add(vertices[nextCornerVertex]);
			triangles.Add(newVertices.Count - 4);
			triangles.Add(newVertices.Count - 3);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 1);
			triangles.Add(newVertices.Count - 4);

			newVertices.Add(vertices[thisWallVertex]);
			newVertices.Add(vertices[thisExtendedWallVertex]);
			newVertices.Add(vertices[nextExtendedWallVertex]);
			newVertices.Add(vertices[nextWallVertex]);
			triangles.Add(newVertices.Count - 4);
			triangles.Add(newVertices.Count - 3);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 1);
			triangles.Add(newVertices.Count - 4);

			newVertices.Add(vertices[thisExtendedWallVertex]);
			newVertices.Add(vertices[thisExtendedCornerVertex]);
			newVertices.Add(vertices[nextExtendedCornerVertex]);
			newVertices.Add(vertices[nextExtendedWallVertex]);
			triangles.Add(newVertices.Count - 4);
			triangles.Add(newVertices.Count - 3);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 2);
			triangles.Add(newVertices.Count - 1);
			triangles.Add(newVertices.Count - 4);
		}

		vertices = newVertices;

		return triangles;
	}
	private GameObject CompleteGameObject(List<Vector3> vertices, List<int> triangles)
	{
		var newGameObject = new GameObject("Room");

		var mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
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