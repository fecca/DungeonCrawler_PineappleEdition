using System.Collections.Generic;
using UnityEngine;

public class CorridorFactory : ModuleFactory
{
	public Module CreateCorridor(Module room, int numberOfCorridorPieces)
	{
		// Clear data
		var vertices = new List<Vector3>();
		var triangles = new List<int>();
		var exitVertices = new List<Vector3>();

		var roomExitsVertices = room.GetExitVertices();

		var direction = (Vector3.Lerp(roomExitsVertices[1], roomExitsVertices[2], 0.5f) - room.transform.position).normalized;

		GenerateVertices(numberOfCorridorPieces, direction, roomExitsVertices, ref vertices);
		GenerateTrianglesWithUniqueVertices(numberOfCorridorPieces, ref vertices, ref triangles);

		return CompleteGameObject(vertices, triangles, exitVertices);
	}

	private void GenerateVertices(int numberOfCorridorPieces, Vector3 direction, List<Vector3> roomExitsVertices, ref List<Vector3> vertices)
	{
		var leftWallVertex = roomExitsVertices[0];
		var leftFloorVertex = roomExitsVertices[1];
		var rightFloorVertex = roomExitsVertices[2];
		var rightWallVertex = roomExitsVertices[3];
		vertices.Add(leftWallVertex);
		vertices.Add(leftFloorVertex);
		vertices.Add(rightFloorVertex);
		vertices.Add(rightWallVertex);

		var corridorWidth = Vector3.Distance(leftFloorVertex, rightFloorVertex);
		var corridorHeight = Vector3.Distance(leftFloorVertex, leftWallVertex);

		for (var i = 0; i < numberOfCorridorPieces; i++)
		{
			var randomYDirection = GetRandomDirectionY(direction.y);
			var randomLeftDirection = new Vector3(GetRandomDirectionX(direction.x), randomYDirection, GetRandomDirectionZ(direction.z));
			var randomRightDirection = new Vector3(GetRandomDirectionX(direction.x), randomYDirection, GetRandomDirectionZ(direction.z));

			var nextLeftFloorVertex = leftFloorVertex + (corridorWidth * randomLeftDirection);
			var nextLeftWallVertex = new Vector3(nextLeftFloorVertex.x, nextLeftFloorVertex.y + corridorHeight, nextLeftFloorVertex.z);
			var nextRightFloorVertex = rightFloorVertex + (corridorWidth * randomRightDirection);
			var nextRightWallVertex = new Vector3(nextRightFloorVertex.x, nextRightFloorVertex.y + corridorHeight, nextRightFloorVertex.z);

			vertices.Add(nextLeftWallVertex);
			vertices.Add(nextLeftFloorVertex);
			vertices.Add(nextRightFloorVertex);
			vertices.Add(nextRightWallVertex);

			leftWallVertex = nextLeftWallVertex;
			leftFloorVertex = nextLeftFloorVertex;
			rightFloorVertex = nextRightFloorVertex;
			rightWallVertex = nextRightWallVertex;

		}
	}
	private void GenerateTrianglesWithUniqueVertices(int numberOfCorridorPieces, ref List<Vector3> vertices, ref List<int> triangles)
	{
		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();
		var numberOfVerticesPerGroup = vertices.Count / numberOfCorridorPieces;

		for (var i = 0; i < numberOfCorridorPieces; i++)
		{
			var thisGroup = (numberOfVerticesPerGroup * i);
			var nextGroup = thisGroup + numberOfVerticesPerGroup;

			var thisLeftWallVertex = thisGroup;
			var thisLeftFloorVertex = thisGroup + 1;
			var thisRightFloorVertex = thisGroup + 2;
			var thisRightWallVertex = thisGroup + 3;

			var nextLeftWallVertex = nextGroup;
			var nextLeftFloorVertex = nextGroup + 1;
			var nextRightFloorVertex = nextGroup + 2;
			var nextRightWallVertex = nextGroup + 3;

			// Left wall
			{
				newVertices.Add(vertices[thisLeftWallVertex]);
				newVertices.Add(vertices[nextLeftWallVertex]);
				newVertices.Add(vertices[nextLeftFloorVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftFloorVertex]);
				newVertices.Add(vertices[thisLeftFloorVertex]);
				newVertices.Add(vertices[thisLeftWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			// Floor
			{
				newVertices.Add(vertices[thisLeftFloorVertex]);
				newVertices.Add(vertices[nextLeftFloorVertex]);
				newVertices.Add(vertices[nextRightFloorVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightFloorVertex]);
				newVertices.Add(vertices[thisRightFloorVertex]);
				newVertices.Add(vertices[thisLeftFloorVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			// Right wall
			{
				newVertices.Add(vertices[thisRightFloorVertex]);
				newVertices.Add(vertices[nextRightFloorVertex]);
				newVertices.Add(vertices[nextRightWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightWallVertex]);
				newVertices.Add(vertices[thisRightWallVertex]);
				newVertices.Add(vertices[thisRightFloorVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			//// Left wall
			//{
			//	newVertices.Add(vertices[currentIndex + 0]);
			//	newVertices.Add(vertices[currentIndex + 2]);
			//	newVertices.Add(vertices[currentIndex + 3]);
			//	newTriangles.Add(newVertices.Count - 3);
			//	newTriangles.Add(newVertices.Count - 2);
			//	newTriangles.Add(newVertices.Count - 1);

			//	newVertices.Add(vertices[currentIndex + 3]);
			//	newVertices.Add(vertices[currentIndex + 1]);
			//	newVertices.Add(vertices[currentIndex + 0]);
			//	newTriangles.Add(newVertices.Count - 3);
			//	newTriangles.Add(newVertices.Count - 2);
			//	newTriangles.Add(newVertices.Count - 1);
			//}

			//// Right wall
			//{
			//	newVertices.Add(vertices[currentIndex + 0]);
			//	newVertices.Add(vertices[currentIndex + 2]);
			//	newVertices.Add(vertices[currentIndex + 3]);
			//	newTriangles.Add(newVertices.Count - 3);
			//	newTriangles.Add(newVertices.Count - 2);
			//	newTriangles.Add(newVertices.Count - 1);

			//	newVertices.Add(vertices[currentIndex + 3]);
			//	newVertices.Add(vertices[currentIndex + 1]);
			//	newVertices.Add(vertices[currentIndex + 0]);
			//	newTriangles.Add(newVertices.Count - 3);
			//	newTriangles.Add(newVertices.Count - 2);
			//	newTriangles.Add(newVertices.Count - 1);
			//}
		}

		vertices = newVertices;
		triangles = newTriangles;
	}
	private float GetRandomDirectionX(float x)
	{
		return x * Random.Range(CorridorModifierIntervalMin, CorridorModifierIntervalMax);
	}
	private float GetRandomDirectionY(float y)
	{
		return y + (3.0f * Random.Range(FloorModifierIntervalMin, FloorModifierIntervalMax));
	}
	private float GetRandomDirectionZ(float z)
	{
		return z * Random.Range(CorridorModifierIntervalMin, CorridorModifierIntervalMax);
	}
}