using System.Collections.Generic;
using UnityEngine;

public class CorridorFactory : ModuleFactory
{
	private const int NumberOfQuads = 20;

	public List<Vector3> CreateVertices(Exit fromExit, Exit toExit)
	{
		var bottomLeftQuadSize = Vector3.Distance(fromExit.BottomLeftExit, toExit.BottomLeftExit) / NumberOfQuads;
		var bottomRightQuadSize = Vector3.Distance(fromExit.BottomRightExit, toExit.BottomRightExit) / NumberOfQuads;
		var topLeftQuadSize = Vector3.Distance(fromExit.TopLeftExit, toExit.TopLeftExit) / NumberOfQuads;
		var topRightQuadSize = Vector3.Distance(fromExit.TopRightExit, toExit.TopRightExit) / NumberOfQuads;

		var vertices = new List<Vector3>();
		for (var i = 0; i < NumberOfQuads; i++)
		{
			var leftOuterFloor = fromExit.BottomLeftExit * (bottomLeftQuadSize * i);
			var leftOuterWall = fromExit.TopLeftExit * (topLeftQuadSize * i);
			var leftWall = fromExit.TopLeftExit * (topLeftQuadSize * i);
			var leftFloor = fromExit.BottomLeftExit * (bottomLeftQuadSize * i);
			var rightFloor = fromExit.BottomRightExit * (bottomRightQuadSize * i);
			var rightWall = fromExit.TopRightExit * (topRightQuadSize * i);
			var rightOuterWall = fromExit.TopRightExit * (topRightQuadSize * i);
			var rightOuterFloor = fromExit.BottomRightExit * (bottomRightQuadSize * i);

			vertices.Add(leftOuterFloor);
			vertices.Add(leftOuterWall);
			vertices.Add(leftWall);
			vertices.Add(leftFloor);
			vertices.Add(rightFloor);
			vertices.Add(rightWall);
			vertices.Add(rightOuterWall);
			vertices.Add(rightOuterFloor);

			var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.localScale = Vector3.one * 0.5f;
			go.transform.position = leftOuterFloor;
			go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.localScale = Vector3.one * 0.5f;
			go.transform.position = leftOuterWall;
			go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.localScale = Vector3.one * 0.5f;
			go.transform.position = leftWall;
			go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.localScale = Vector3.one * 0.5f;
			go.transform.position = leftFloor;
			go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.localScale = Vector3.one * 0.5f;
			go.transform.position = rightFloor;
			go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.localScale = Vector3.one * 0.5f;
			go.transform.position = rightWall;
			go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.localScale = Vector3.one * 0.5f;
			go.transform.position = rightOuterWall;
			go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.transform.localScale = Vector3.one * 0.5f;
			go.transform.position = rightOuterFloor;
		}

		return vertices;
	}
	public List<int> CreateTriangles(Corridor corridor)
	{
		var vertices = corridor.Vertices;
		var newVertices = new List<Vector3>();
		var triangles = new List<int>();
		var numberOfVerticesPerGroup = vertices.Count / (corridor.NumberOfQuads + 1);

		for (var i = 0; i < corridor.NumberOfQuads; i++)
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
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftOutsideWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Left roof
			{
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				newVertices.Add(vertices[nextLeftOutsideWallVertex]);
				newVertices.Add(vertices[nextLeftWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftWallVertex]);
				newVertices.Add(vertices[thisLeftWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Left wall
			{
				newVertices.Add(vertices[thisLeftWallVertex]);
				newVertices.Add(vertices[nextLeftWallVertex]);
				newVertices.Add(vertices[nextLeftFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftFloorVertex]);
				newVertices.Add(vertices[thisLeftFloorVertex]);
				newVertices.Add(vertices[thisLeftWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Floor
			{
				newVertices.Add(vertices[thisLeftFloorVertex]);
				newVertices.Add(vertices[nextLeftFloorVertex]);
				newVertices.Add(vertices[nextRightFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightFloorVertex]);
				newVertices.Add(vertices[thisRightFloorVertex]);
				newVertices.Add(vertices[thisLeftFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Right wall
			{
				newVertices.Add(vertices[thisRightFloorVertex]);
				newVertices.Add(vertices[nextRightFloorVertex]);
				newVertices.Add(vertices[nextRightWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightWallVertex]);
				newVertices.Add(vertices[thisRightWallVertex]);
				newVertices.Add(vertices[thisRightFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Right roof
			{
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				newVertices.Add(vertices[thisRightWallVertex]);
				newVertices.Add(vertices[nextRightWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightWallVertex]);
				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Right outside wall
			{
				newVertices.Add(vertices[thisRightOutsideFloorVertex]);
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				newVertices.Add(vertices[nextRightOutsideFloorVertex]);
				newVertices.Add(vertices[thisRightOutsideFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}
		}

		vertices = newVertices;

		return triangles;
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