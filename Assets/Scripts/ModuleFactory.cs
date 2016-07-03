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

	protected Module CompleteGameObject(List<Vector3> vertices, List<int> triangles, List<Vector3> exitVertices)
	{
		var newGameObject = new GameObject("Module");

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

		var room = newGameObject.AddComponent<Module>();
		room.AddExitVertices(exitVertices);

		return room;
	}

	//private void GenerateRoomTrianglesWithSharedVertices(int numberOfCorners)
	//{
	//	var newVertices = new List<Vector3>();
	//	var newTriangles = new List<int>();
	//	var numberOfVerticesPerGroup = _vertices.Count / numberOfCorners;
	//	for (var i = 0; i < numberOfCorners; i++)
	//	{
	//		var thisGroup = (numberOfVerticesPerGroup * i);
	//		var nextGroup = thisGroup + numberOfVerticesPerGroup >= _vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

	//		var thisOrigoVertex = thisGroup;
	//		var thisFloorVertex1 = thisGroup + 1;
	//		var thisFloorVertex2 = thisGroup + 2;
	//		var thisCornerVertex = thisGroup + 3;
	//		var thisWallVertex = thisGroup + 4;
	//		var thisOutsideWallVertex = thisGroup + 5;
	//		var thisOutsideCornerVertex = thisGroup + 6;

	//		var nextFloorVertex1 = nextGroup + 1;
	//		var nextFloorVertex2 = nextGroup + 2;
	//		var nextCornerVertex = nextGroup + 3;
	//		var nextWallVertex = nextGroup + 4;
	//		var nextOutsideWallVertex = nextGroup + 5;
	//		var nextOutsideCornerVertex = nextGroup + 6;

	//		newVertices.Add(_vertices[thisOrigoVertex]);
	//		newVertices.Add(_vertices[thisFloorVertex1]);
	//		newVertices.Add(_vertices[nextFloorVertex1]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[thisFloorVertex1]);
	//		newVertices.Add(_vertices[thisFloorVertex2]);
	//		newVertices.Add(_vertices[nextFloorVertex2]);
	//		newVertices.Add(_vertices[nextFloorVertex1]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);

	//		newVertices.Add(_vertices[thisCornerVertex]);
	//		newVertices.Add(_vertices[nextCornerVertex]);
	//		newVertices.Add(_vertices[nextFloorVertex2]);
	//		newVertices.Add(_vertices[thisFloorVertex2]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);

	//		newVertices.Add(_vertices[thisCornerVertex]);
	//		newVertices.Add(_vertices[thisWallVertex]);
	//		newVertices.Add(_vertices[nextWallVertex]);
	//		newVertices.Add(_vertices[nextCornerVertex]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);

	//		newVertices.Add(_vertices[thisWallVertex]);
	//		newVertices.Add(_vertices[thisOutsideWallVertex]);
	//		newVertices.Add(_vertices[nextOutsideWallVertex]);
	//		newVertices.Add(_vertices[nextWallVertex]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);

	//		newVertices.Add(_vertices[thisOutsideWallVertex]);
	//		newVertices.Add(_vertices[thisOutsideCornerVertex]);
	//		newVertices.Add(_vertices[nextOutsideCornerVertex]);
	//		newVertices.Add(_vertices[nextOutsideWallVertex]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);
	//	}

	//	_vertices = newVertices;
	//	_triangles = newTriangles;
	//}
	//private void GenerateRoomTrianglesWithSharedWallVertices(int numberOfCorners)
	//{
	//	var newVertices = new List<Vector3>();
	//	var newTriangles = new List<int>();
	//	var numberOfVerticesPerGroup = _vertices.Count / numberOfCorners;
	//	for (var i = 0; i < numberOfCorners; i++)
	//	{
	//		var thisGroup = (numberOfVerticesPerGroup * i);
	//		var nextGroup = thisGroup + numberOfVerticesPerGroup >= _vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

	//		var thisOrigoVertex = thisGroup;
	//		var thisFloorVertex1 = thisGroup + 1;
	//		var thisFloorVertex2 = thisGroup + 2;
	//		var thisCornerVertex = thisGroup + 3;
	//		var thisWallVertex = thisGroup + 4;
	//		var thisOutsideWallVertex = thisGroup + 5;
	//		var thisOutsideCornerVertex = thisGroup + 6;

	//		var nextFloorVertex1 = nextGroup + 1;
	//		var nextFloorVertex2 = nextGroup + 2;
	//		var nextCornerVertex = nextGroup + 3;
	//		var nextWallVertex = nextGroup + 4;
	//		var nextOutsideWallVertex = nextGroup + 5;
	//		var nextOutsideCornerVertex = nextGroup + 6;

	//		newVertices.Add(_vertices[thisOrigoVertex]);
	//		newVertices.Add(_vertices[thisFloorVertex1]);
	//		newVertices.Add(_vertices[nextFloorVertex1]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[thisFloorVertex1]);
	//		newVertices.Add(_vertices[thisFloorVertex2]);
	//		newVertices.Add(_vertices[nextFloorVertex2]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[nextFloorVertex2]);
	//		newVertices.Add(_vertices[nextFloorVertex1]);
	//		newVertices.Add(_vertices[thisFloorVertex1]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[thisCornerVertex]);
	//		newVertices.Add(_vertices[nextCornerVertex]);
	//		newVertices.Add(_vertices[nextFloorVertex2]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[nextFloorVertex2]);
	//		newVertices.Add(_vertices[thisFloorVertex2]);
	//		newVertices.Add(_vertices[thisCornerVertex]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[thisCornerVertex]);
	//		newVertices.Add(_vertices[thisWallVertex]);
	//		newVertices.Add(_vertices[nextWallVertex]);
	//		newVertices.Add(_vertices[nextCornerVertex]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);

	//		newVertices.Add(_vertices[thisWallVertex]);
	//		newVertices.Add(_vertices[thisOutsideWallVertex]);
	//		newVertices.Add(_vertices[nextOutsideWallVertex]);
	//		newVertices.Add(_vertices[nextWallVertex]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);

	//		newVertices.Add(_vertices[thisOutsideWallVertex]);
	//		newVertices.Add(_vertices[thisOutsideCornerVertex]);
	//		newVertices.Add(_vertices[nextOutsideCornerVertex]);
	//		newVertices.Add(_vertices[nextOutsideWallVertex]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);
	//	}

	//	_vertices = newVertices;
	//	_triangles = newTriangles;
	//}
	//private void GenerateRoomTrianglesWithSharedFloorVertices(int numberOfCorners)
	//{
	//	var newVertices = new List<Vector3>();
	//	var newTriangles = new List<int>();
	//	var numberOfVerticesPerGroup = _vertices.Count / numberOfCorners;
	//	for (var i = 0; i < numberOfCorners; i++)
	//	{
	//		var thisGroup = (numberOfVerticesPerGroup * i);
	//		var nextGroup = thisGroup + numberOfVerticesPerGroup >= _vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

	//		var thisOrigoVertex = thisGroup;
	//		var thisFloorVertex1 = thisGroup + 1;
	//		var thisFloorVertex2 = thisGroup + 2;
	//		var thisCornerVertex = thisGroup + 3;
	//		var thisWallVertex = thisGroup + 4;
	//		var thisOutsideWallVertex = thisGroup + 5;
	//		var thisOutsideCornerVertex = thisGroup + 6;

	//		var nextFloorVertex1 = nextGroup + 1;
	//		var nextFloorVertex2 = nextGroup + 2;
	//		var nextCornerVertex = nextGroup + 3;
	//		var nextWallVertex = nextGroup + 4;
	//		var nextOutsideWallVertex = nextGroup + 5;
	//		var nextOutsideCornerVertex = nextGroup + 6;

	//		newVertices.Add(_vertices[thisOrigoVertex]);
	//		newVertices.Add(_vertices[thisFloorVertex1]);
	//		newVertices.Add(_vertices[nextFloorVertex1]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);


	//		newVertices.Add(_vertices[thisFloorVertex1]);
	//		newVertices.Add(_vertices[thisFloorVertex2]);
	//		newVertices.Add(_vertices[nextFloorVertex2]);
	//		newVertices.Add(_vertices[nextFloorVertex1]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);

	//		newVertices.Add(_vertices[thisCornerVertex]);
	//		newVertices.Add(_vertices[nextCornerVertex]);
	//		newVertices.Add(_vertices[nextFloorVertex2]);
	//		newVertices.Add(_vertices[thisFloorVertex2]);
	//		newTriangles.Add(newVertices.Count - 4);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//		newTriangles.Add(newVertices.Count - 4);

	//		newVertices.Add(_vertices[thisCornerVertex]);
	//		newVertices.Add(_vertices[thisWallVertex]);
	//		newVertices.Add(_vertices[nextWallVertex]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[nextWallVertex]);
	//		newVertices.Add(_vertices[nextCornerVertex]);
	//		newVertices.Add(_vertices[thisCornerVertex]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[thisWallVertex]);
	//		newVertices.Add(_vertices[thisOutsideWallVertex]);
	//		newVertices.Add(_vertices[nextOutsideWallVertex]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[nextOutsideWallVertex]);
	//		newVertices.Add(_vertices[nextWallVertex]);
	//		newVertices.Add(_vertices[thisWallVertex]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[thisOutsideWallVertex]);
	//		newVertices.Add(_vertices[thisOutsideCornerVertex]);
	//		newVertices.Add(_vertices[nextOutsideCornerVertex]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);

	//		newVertices.Add(_vertices[nextOutsideCornerVertex]);
	//		newVertices.Add(_vertices[nextOutsideWallVertex]);
	//		newVertices.Add(_vertices[thisOutsideWallVertex]);
	//		newTriangles.Add(newVertices.Count - 3);
	//		newTriangles.Add(newVertices.Count - 2);
	//		newTriangles.Add(newVertices.Count - 1);
	//	}

	//	_vertices = newVertices;
	//	_triangles = newTriangles;
	//}
}