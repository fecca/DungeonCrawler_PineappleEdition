using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporary, for testing purposes
/// Remove when no longer needed
/// </summary>
public enum RoomArea
{
	None,
	Floor,
	Wall,
	All
}

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

	private GameObject _gameObject;
	private Vector3 _position = Vector3.zero;
	private List<Vector3> _vertices;
	private List<int> _triangles;
	private List<Vector3> _exitVertices;
	private RoomArea _sharedVertices;

	public GameObject CreateRoom(Corridor corridor, int numberOfCorners, int radius, int thickness, int height)
	{
		if (corridor != null)
		{
			_position = corridor.transform.position;
		}
		// Clear data
		_vertices = new List<Vector3>();
		_triangles = new List<int>();
		_exitVertices = new List<Vector3>();

		GenerateVertices(numberOfCorners, radius, thickness, height);

		switch (_sharedVertices)
		{
			case RoomArea.None:
				GenerateTrianglesWithUniqueVertices(numberOfCorners);
				break;
			case RoomArea.Floor:
				GenerateTrianglesWithSharedFloorVertices(numberOfCorners);
				break;
			case RoomArea.Wall:
				GenerateTrianglesWithSharedWallVertices(numberOfCorners);
				break;
			case RoomArea.All:
				GenerateTrianglesWithSharedVertices(numberOfCorners);
				break;
		}

		CompleteGameObject();

		return _gameObject;
	}

	private void GenerateVertices(int numberOfCorners, int radius, int thickness, int height)
	{
		var angle = Random.Range(0f, 360f);
		var angleStep = 360f / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var modifiedAngle = angle + (angleStep * 0.5f) * Random.Range(AngleModifierIntervalMin, AngleModifierIntervalMax);
			var modifiedRadius = radius * Random.Range(RadiusModifierIntervalMin, RadiusModifierIntervalMax);
			var x = Mathf.Sin(Mathf.Deg2Rad * modifiedAngle) * modifiedRadius;
			var z = Mathf.Cos(Mathf.Deg2Rad * modifiedAngle) * modifiedRadius;

			// Origo
			_vertices.Add(_position);

			// Floor vertex 1
			var floorVertex1 = (new Vector3(x * 0.33f, _position.y + Random.Range(FloorModifierIntervalMin, FloorModifierIntervalMax), z * 0.33f) + _position);
			_vertices.Add(floorVertex1);

			// Floor vertex 2
			var floorVertex2 = (new Vector3(x * 0.66f, _position.y + Random.Range(FloorModifierIntervalMin, FloorModifierIntervalMax), z * 0.66f) + _position);
			_vertices.Add(floorVertex2);

			// Corner vertex
			var cornerVertex = new Vector3(x, _position.y, z) + _position;
			_vertices.Add(cornerVertex);

			var directionNormalized = (cornerVertex - _position).normalized;

			// Wall vertex
			var wallVertex = cornerVertex + (directionNormalized * Random.Range(WallModifierIntervalMin, WallModifierIntervalMax)) + (Vector3.up * height);
			_vertices.Add(wallVertex);

			// Outside wall vertex
			var outsideWallVertex = wallVertex + directionNormalized + (directionNormalized * Random.Range(WallModifierIntervalMin, WallModifierIntervalMax));
			_vertices.Add(outsideWallVertex);

			// Outside corner vertex
			var outsideCornerVertex = directionNormalized + cornerVertex;
			_vertices.Add(outsideCornerVertex);

			angle += 360f / numberOfCorners;
		}
	}
	private void GenerateTrianglesWithUniqueVertices(int numberOfCorners)
	{
		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();
		var numberOfVerticesPerGroup = _vertices.Count / numberOfCorners;

		for (var i = 0; i < numberOfCorners; i++)
		{
			var thisGroup = (numberOfVerticesPerGroup * i);
			var nextGroup = thisGroup + numberOfVerticesPerGroup >= _vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

			var thisOrigoVertex = thisGroup;
			var thisFloorVertex1 = thisGroup + 1;
			var thisFloorVertex2 = thisGroup + 2;
			var thisCornerVertex = thisGroup + 3;
			var thisWallVertex = thisGroup + 4;
			var thisOutsideWallVertex = thisGroup + 5;
			var thisOutsideCornerVertex = thisGroup + 6;

			var nextFloorVertex1 = nextGroup + 1;
			var nextFloorVertex2 = nextGroup + 2;
			var nextCornerVertex = nextGroup + 3;
			var nextWallVertex = nextGroup + 4;
			var nextOutsideWallVertex = nextGroup + 5;
			var nextOutsideCornerVertex = nextGroup + 6;

			// First floor triangle
			newVertices.Add(_vertices[thisOrigoVertex]);
			newVertices.Add(_vertices[thisFloorVertex1]);
			newVertices.Add(_vertices[nextFloorVertex1]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// Second floor triangle 1
			newVertices.Add(_vertices[thisFloorVertex1]);
			newVertices.Add(_vertices[thisFloorVertex2]);
			newVertices.Add(_vertices[nextFloorVertex2]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// Second floor triangle 2
			newVertices.Add(_vertices[nextFloorVertex2]);
			newVertices.Add(_vertices[nextFloorVertex1]);
			newVertices.Add(_vertices[thisFloorVertex1]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// Third floor triangle 1
			newVertices.Add(_vertices[thisCornerVertex]);
			newVertices.Add(_vertices[nextCornerVertex]);
			newVertices.Add(_vertices[nextFloorVertex2]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// Third floor triangle 2
			newVertices.Add(_vertices[nextFloorVertex2]);
			newVertices.Add(_vertices[thisFloorVertex2]);
			newVertices.Add(_vertices[thisCornerVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// Save vertices for exit and skip drawing wall
			if (_exitVertices.IsEmpty())
			{
				_exitVertices.Add(_vertices[thisCornerVertex]);
				_exitVertices.Add(_vertices[thisWallVertex]);
				_exitVertices.Add(_vertices[thisOutsideWallVertex]);
				_exitVertices.Add(_vertices[thisOutsideCornerVertex]);

				_exitVertices.Add(_vertices[nextCornerVertex]);
				_exitVertices.Add(_vertices[nextWallVertex]);
				_exitVertices.Add(_vertices[nextOutsideWallVertex]);
				_exitVertices.Add(_vertices[nextOutsideCornerVertex]);

				continue;
			}

			// First inside wall triangle 1
			newVertices.Add(_vertices[thisCornerVertex]);
			newVertices.Add(_vertices[thisWallVertex]);
			newVertices.Add(_vertices[nextWallVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// First inside wall triangle 2
			newVertices.Add(_vertices[nextWallVertex]);
			newVertices.Add(_vertices[nextCornerVertex]);
			newVertices.Add(_vertices[thisCornerVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// First roof triangle 1
			newVertices.Add(_vertices[thisWallVertex]);
			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// First roof triangle 2
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newVertices.Add(_vertices[nextWallVertex]);
			newVertices.Add(_vertices[thisWallVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// First outside wall triangle 1
			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newVertices.Add(_vertices[thisOutsideCornerVertex]);
			newVertices.Add(_vertices[nextOutsideCornerVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			// First outside wall triangle 2
			newVertices.Add(_vertices[nextOutsideCornerVertex]);
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
		}

		//if (corridorVertices.Count > 0)
		//{
		//	newVertices.Add(corridorVertices[0]);
		//	newVertices.Add(corridorVertices[1]);
		//	newVertices.Add(corridorVertices[2]);
		//	triangles.Add(newVertices.Count - 3);
		//	triangles.Add(newVertices.Count - 2);
		//	triangles.Add(newVertices.Count - 1);

		//	newVertices.Add(corridorVertices[2]);
		//	newVertices.Add(corridorVertices[3]);
		//	newVertices.Add(corridorVertices[0]);
		//	triangles.Add(newVertices.Count - 3);
		//	triangles.Add(newVertices.Count - 2);
		//	triangles.Add(newVertices.Count - 1);

		//	newVertices.Add(corridorVertices[0]);
		//	newVertices.Add(corridorVertices[3]);
		//	newVertices.Add(corridorVertices[7]);
		//	triangles.Add(newVertices.Count - 3);
		//	triangles.Add(newVertices.Count - 2);
		//	triangles.Add(newVertices.Count - 1);

		//	newVertices.Add(corridorVertices[7]);
		//	newVertices.Add(corridorVertices[4]);
		//	newVertices.Add(corridorVertices[0]);
		//	triangles.Add(newVertices.Count - 3);
		//	triangles.Add(newVertices.Count - 2);
		//	triangles.Add(newVertices.Count - 1);

		//	newVertices.Add(corridorVertices[4]);
		//	newVertices.Add(corridorVertices[7]);
		//	newVertices.Add(corridorVertices[6]);
		//	triangles.Add(newVertices.Count - 3);
		//	triangles.Add(newVertices.Count - 2);
		//	triangles.Add(newVertices.Count - 1);

		//	newVertices.Add(corridorVertices[6]);
		//	newVertices.Add(corridorVertices[5]);
		//	newVertices.Add(corridorVertices[4]);
		//	triangles.Add(newVertices.Count - 3);
		//	triangles.Add(newVertices.Count - 2);
		//	triangles.Add(newVertices.Count - 1);
		//}

		_vertices = newVertices;
		_triangles = newTriangles;
	}
	private void GenerateTrianglesWithSharedVertices(int numberOfCorners)
	{
		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();
		var numberOfVerticesPerGroup = _vertices.Count / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var thisGroup = (numberOfVerticesPerGroup * i);
			var nextGroup = thisGroup + numberOfVerticesPerGroup >= _vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

			var thisOrigoVertex = thisGroup;
			var thisFloorVertex1 = thisGroup + 1;
			var thisFloorVertex2 = thisGroup + 2;
			var thisCornerVertex = thisGroup + 3;
			var thisWallVertex = thisGroup + 4;
			var thisOutsideWallVertex = thisGroup + 5;
			var thisOutsideCornerVertex = thisGroup + 6;

			var nextFloorVertex1 = nextGroup + 1;
			var nextFloorVertex2 = nextGroup + 2;
			var nextCornerVertex = nextGroup + 3;
			var nextWallVertex = nextGroup + 4;
			var nextOutsideWallVertex = nextGroup + 5;
			var nextOutsideCornerVertex = nextGroup + 6;

			newVertices.Add(_vertices[thisOrigoVertex]);
			newVertices.Add(_vertices[thisFloorVertex1]);
			newVertices.Add(_vertices[nextFloorVertex1]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[thisFloorVertex1]);
			newVertices.Add(_vertices[thisFloorVertex2]);
			newVertices.Add(_vertices[nextFloorVertex2]);
			newVertices.Add(_vertices[nextFloorVertex1]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);

			newVertices.Add(_vertices[thisCornerVertex]);
			newVertices.Add(_vertices[nextCornerVertex]);
			newVertices.Add(_vertices[nextFloorVertex2]);
			newVertices.Add(_vertices[thisFloorVertex2]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);

			newVertices.Add(_vertices[thisCornerVertex]);
			newVertices.Add(_vertices[thisWallVertex]);
			newVertices.Add(_vertices[nextWallVertex]);
			newVertices.Add(_vertices[nextCornerVertex]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);

			newVertices.Add(_vertices[thisWallVertex]);
			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newVertices.Add(_vertices[nextWallVertex]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);

			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newVertices.Add(_vertices[thisOutsideCornerVertex]);
			newVertices.Add(_vertices[nextOutsideCornerVertex]);
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);
		}

		_vertices = newVertices;
		_triangles = newTriangles;
	}
	private void GenerateTrianglesWithSharedWallVertices(int numberOfCorners)
	{
		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();
		var numberOfVerticesPerGroup = _vertices.Count / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var thisGroup = (numberOfVerticesPerGroup * i);
			var nextGroup = thisGroup + numberOfVerticesPerGroup >= _vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

			var thisOrigoVertex = thisGroup;
			var thisFloorVertex1 = thisGroup + 1;
			var thisFloorVertex2 = thisGroup + 2;
			var thisCornerVertex = thisGroup + 3;
			var thisWallVertex = thisGroup + 4;
			var thisOutsideWallVertex = thisGroup + 5;
			var thisOutsideCornerVertex = thisGroup + 6;

			var nextFloorVertex1 = nextGroup + 1;
			var nextFloorVertex2 = nextGroup + 2;
			var nextCornerVertex = nextGroup + 3;
			var nextWallVertex = nextGroup + 4;
			var nextOutsideWallVertex = nextGroup + 5;
			var nextOutsideCornerVertex = nextGroup + 6;

			newVertices.Add(_vertices[thisOrigoVertex]);
			newVertices.Add(_vertices[thisFloorVertex1]);
			newVertices.Add(_vertices[nextFloorVertex1]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[thisFloorVertex1]);
			newVertices.Add(_vertices[thisFloorVertex2]);
			newVertices.Add(_vertices[nextFloorVertex2]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[nextFloorVertex2]);
			newVertices.Add(_vertices[nextFloorVertex1]);
			newVertices.Add(_vertices[thisFloorVertex1]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[thisCornerVertex]);
			newVertices.Add(_vertices[nextCornerVertex]);
			newVertices.Add(_vertices[nextFloorVertex2]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[nextFloorVertex2]);
			newVertices.Add(_vertices[thisFloorVertex2]);
			newVertices.Add(_vertices[thisCornerVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[thisCornerVertex]);
			newVertices.Add(_vertices[thisWallVertex]);
			newVertices.Add(_vertices[nextWallVertex]);
			newVertices.Add(_vertices[nextCornerVertex]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);

			newVertices.Add(_vertices[thisWallVertex]);
			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newVertices.Add(_vertices[nextWallVertex]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);

			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newVertices.Add(_vertices[thisOutsideCornerVertex]);
			newVertices.Add(_vertices[nextOutsideCornerVertex]);
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);
		}

		_vertices = newVertices;
		_triangles = newTriangles;
	}
	private void GenerateTrianglesWithSharedFloorVertices(int numberOfCorners)
	{
		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();
		var numberOfVerticesPerGroup = _vertices.Count / numberOfCorners;
		for (var i = 0; i < numberOfCorners; i++)
		{
			var thisGroup = (numberOfVerticesPerGroup * i);
			var nextGroup = thisGroup + numberOfVerticesPerGroup >= _vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

			var thisOrigoVertex = thisGroup;
			var thisFloorVertex1 = thisGroup + 1;
			var thisFloorVertex2 = thisGroup + 2;
			var thisCornerVertex = thisGroup + 3;
			var thisWallVertex = thisGroup + 4;
			var thisOutsideWallVertex = thisGroup + 5;
			var thisOutsideCornerVertex = thisGroup + 6;

			var nextFloorVertex1 = nextGroup + 1;
			var nextFloorVertex2 = nextGroup + 2;
			var nextCornerVertex = nextGroup + 3;
			var nextWallVertex = nextGroup + 4;
			var nextOutsideWallVertex = nextGroup + 5;
			var nextOutsideCornerVertex = nextGroup + 6;

			newVertices.Add(_vertices[thisOrigoVertex]);
			newVertices.Add(_vertices[thisFloorVertex1]);
			newVertices.Add(_vertices[nextFloorVertex1]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);


			newVertices.Add(_vertices[thisFloorVertex1]);
			newVertices.Add(_vertices[thisFloorVertex2]);
			newVertices.Add(_vertices[nextFloorVertex2]);
			newVertices.Add(_vertices[nextFloorVertex1]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);

			newVertices.Add(_vertices[thisCornerVertex]);
			newVertices.Add(_vertices[nextCornerVertex]);
			newVertices.Add(_vertices[nextFloorVertex2]);
			newVertices.Add(_vertices[thisFloorVertex2]);
			newTriangles.Add(newVertices.Count - 4);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
			newTriangles.Add(newVertices.Count - 4);

			newVertices.Add(_vertices[thisCornerVertex]);
			newVertices.Add(_vertices[thisWallVertex]);
			newVertices.Add(_vertices[nextWallVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[nextWallVertex]);
			newVertices.Add(_vertices[nextCornerVertex]);
			newVertices.Add(_vertices[thisCornerVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[thisWallVertex]);
			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newVertices.Add(_vertices[nextWallVertex]);
			newVertices.Add(_vertices[thisWallVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newVertices.Add(_vertices[thisOutsideCornerVertex]);
			newVertices.Add(_vertices[nextOutsideCornerVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);

			newVertices.Add(_vertices[nextOutsideCornerVertex]);
			newVertices.Add(_vertices[nextOutsideWallVertex]);
			newVertices.Add(_vertices[thisOutsideWallVertex]);
			newTriangles.Add(newVertices.Count - 3);
			newTriangles.Add(newVertices.Count - 2);
			newTriangles.Add(newVertices.Count - 1);
		}

		_vertices = newVertices;
		_triangles = newTriangles;
	}
	private void CompleteGameObject()
	{
		_gameObject = new GameObject("Room");

		var room = _gameObject.AddComponent<Room>();
		room.AddExitVertices(_exitVertices);

		var mesh = new Mesh();
		mesh.vertices = _vertices.ToArray();
		mesh.triangles = _triangles.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		var meshFilter = _gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		var meshRenderer = _gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = Resources.Load<Material>("WallMaterial_Unlit");

		var meshCollider = _gameObject.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = mesh;
	}
}