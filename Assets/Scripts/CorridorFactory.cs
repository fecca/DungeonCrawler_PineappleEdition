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

		GenerateVertices(numberOfCorridorPieces, room.transform.position, roomExitsVertices, ref vertices);
		GenerateTrianglesWithUniqueVertices(numberOfCorridorPieces, ref vertices, ref triangles);

		return CompleteGameObject(vertices, triangles, exitVertices);
	}

	private void GenerateVertices(int numberOfCorridorPieces, Vector3 roomPosition, List<Vector3> roomExitsVertices, ref List<Vector3> vertices)
	{
		var leftWallVertex = roomExitsVertices[0];
		var leftFloorVertex = roomExitsVertices[1];
		var rightFloorVertex = roomExitsVertices[2];
		var rightWallVertex = roomExitsVertices[3];

		var forwardDirection = (Vector3.Lerp(leftFloorVertex, rightFloorVertex, 0.5f) - roomPosition).normalized;
		var sidewaysDirection = (leftWallVertex - rightWallVertex).normalized;
		var corridorWidth = Vector3.Distance(leftFloorVertex, rightFloorVertex);
		var corridorHeight = Vector3.Distance(leftFloorVertex, leftWallVertex);

		var leftOutsideFloorVertex = leftFloorVertex;
		var leftOutsideWallVertex = leftWallVertex;
		var rightOutsideWallVertex = rightWallVertex;
		var rightOutsideFloorVertex = rightFloorVertex;

		vertices.Add(leftOutsideFloorVertex);
		vertices.Add(leftOutsideWallVertex);
		vertices.Add(leftWallVertex);
		vertices.Add(leftFloorVertex);
		vertices.Add(rightFloorVertex);
		vertices.Add(rightWallVertex);
		vertices.Add(rightOutsideWallVertex);
		vertices.Add(rightOutsideFloorVertex);

		for (var i = 0; i < numberOfCorridorPieces; i++)
		{
			var randomXDirection = GetRandomDirectionX(forwardDirection.x);
			var randomYDirection = GetRandomDirectionY(forwardDirection.y);
			var randomZDirection = GetRandomDirectionZ(forwardDirection.z);
			var randomLeftDirection = new Vector3(randomXDirection, randomYDirection, randomZDirection);
			var randomRightDirection = new Vector3(randomXDirection, randomYDirection, randomZDirection);

			var nextLeftFloorVertex = leftFloorVertex + (corridorWidth * randomLeftDirection);
			var nextLeftWallVertex = new Vector3(nextLeftFloorVertex.x, nextLeftFloorVertex.y + corridorHeight, nextLeftFloorVertex.z);
			var nextRightFloorVertex = rightFloorVertex + (corridorWidth * randomRightDirection);
			var nextRightWallVertex = new Vector3(nextRightFloorVertex.x, nextRightFloorVertex.y + corridorHeight, nextRightFloorVertex.z);

			var nextLeftOutsideFloorVertex = nextLeftFloorVertex + (sidewaysDirection * 1);
			var nextLeftOutsideWallVertex = nextLeftWallVertex + (sidewaysDirection * 1);
			var nextRightOutsideWallVertex = nextRightWallVertex - (sidewaysDirection * 1);
			var nextRightOutsideFloorVertex = nextRightFloorVertex - (sidewaysDirection * 1);

			vertices.Add(nextLeftOutsideFloorVertex);
			vertices.Add(nextLeftOutsideWallVertex);
			vertices.Add(nextLeftWallVertex);
			vertices.Add(nextLeftFloorVertex);
			vertices.Add(nextRightFloorVertex);
			vertices.Add(nextRightWallVertex);
			vertices.Add(nextRightOutsideWallVertex);
			vertices.Add(nextRightOutsideFloorVertex);

			leftOutsideFloorVertex = nextLeftOutsideFloorVertex;
			leftOutsideWallVertex = nextLeftOutsideWallVertex;
			leftWallVertex = nextLeftWallVertex;
			leftFloorVertex = nextLeftFloorVertex;
			rightFloorVertex = nextRightFloorVertex;
			rightWallVertex = nextRightWallVertex;
			rightOutsideWallVertex = nextRightOutsideWallVertex;
			rightOutsideFloorVertex = nextRightOutsideFloorVertex;

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

			var thisLeftOutsideFloorVertex = thisGroup;
			var thisLeftOutsideWallVertex = thisGroup + 1;
			var thisLeftWallVertex = thisGroup + 2;
			var thisLeftFloorVertex = thisGroup + 3;
			var thisRightFloorVertex = thisGroup + 4;
			var thisRightWallVertex = thisGroup + 5;
			var thisRightOutsideWallVertex = thisGroup + 6;
			var thisRightOutsideFloorVertex = thisGroup + 7;

			var nextLeftOutsideFloorVertex = nextGroup;
			var nextLeftOutsideWallVertex = nextGroup + 1;
			var nextLeftWallVertex = nextGroup + 2;
			var nextLeftFloorVertex = nextGroup + 3;
			var nextRightFloorVertex = nextGroup + 4;
			var nextRightWallVertex = nextGroup + 5;
			var nextRightOutsideWallVertex = nextGroup + 6;
			var nextRightOutsideFloorVertex = nextGroup + 7;

			// Left outside wall
			{
				newVertices.Add(vertices[thisLeftOutsideFloorVertex]);
				newVertices.Add(vertices[nextLeftOutsideFloorVertex]);
				newVertices.Add(vertices[nextLeftOutsideWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftOutsideWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideFloorVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			// Left roof
			{
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				newVertices.Add(vertices[nextLeftOutsideWallVertex]);
				newVertices.Add(vertices[nextLeftWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftWallVertex]);
				newVertices.Add(vertices[thisLeftWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

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

			// Right roof
			{
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				newVertices.Add(vertices[thisRightWallVertex]);
				newVertices.Add(vertices[nextRightWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightWallVertex]);
				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			// Right outside wall
			{
				newVertices.Add(vertices[thisRightOutsideFloorVertex]);
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				newVertices.Add(vertices[nextRightOutsideFloorVertex]);
				newVertices.Add(vertices[thisRightOutsideFloorVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}
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