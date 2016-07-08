using System.Collections.Generic;
using UnityEngine;

public class CorridorFactory : ModuleFactory
{
	public List<Vector3> CreateVertices(Exit fromExit, Exit toExit, int numberOfQuads)
	{
		var bottomLeftQuadSize = Vector3.Distance(fromExit.BottomLeftExit, toExit.BottomRightExit) / numberOfQuads;
		var topLeftQuadSize = Vector3.Distance(fromExit.TopLeftExit, toExit.TopRightExit) / numberOfQuads;
		var bottomRightQuadSize = Vector3.Distance(fromExit.BottomRightExit, toExit.BottomLeftExit) / numberOfQuads;
		var topRightQuadSize = Vector3.Distance(fromExit.TopRightExit, toExit.TopLeftExit) / numberOfQuads;

		var bottomLeftDirection = (toExit.BottomRightExit - fromExit.BottomLeftExit).normalized;
		var topLeftDirection = (toExit.TopRightExit - fromExit.TopLeftExit).normalized;
		var topRightDirection = (toExit.TopLeftExit - fromExit.TopRightExit).normalized;
		var bottomRightDirection = (toExit.BottomLeftExit - fromExit.BottomRightExit).normalized;

		var thicknessDirection = (fromExit.BottomLeftExit - fromExit.BottomRightExit).normalized;

		var leftOuterFloor = fromExit.BottomLeftExit;
		var leftOuterWall = fromExit.TopLeftExit;
		var leftWall = fromExit.TopLeftExit;
		var leftFloor = fromExit.BottomLeftExit;
		var rightFloor = fromExit.BottomRightExit;
		var rightWall = fromExit.TopRightExit;
		var rightOuterWall = fromExit.TopRightExit;
		var rightOuterFloor = fromExit.BottomRightExit;

		var vertices = new List<Vector3>();
		vertices.Add(leftOuterFloor);
		vertices.Add(leftOuterWall);
		vertices.Add(leftWall);
		vertices.Add(leftFloor);
		vertices.Add(rightFloor);
		vertices.Add(rightWall);
		vertices.Add(rightOuterWall);
		vertices.Add(rightOuterFloor);

		//var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.localScale = Vector3.one * 0.5f;
		//go.transform.position = leftOuterFloor;
		//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.localScale = Vector3.one * 0.5f;
		//go.transform.position = leftOuterWall;
		//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.localScale = Vector3.one * 0.5f;
		//go.transform.position = leftWall;
		//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.localScale = Vector3.one * 0.5f;
		//go.transform.position = leftFloor;
		//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.localScale = Vector3.one * 0.5f;
		//go.transform.position = rightFloor;
		//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.localScale = Vector3.one * 0.5f;
		//go.transform.position = rightWall;
		//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.localScale = Vector3.one * 0.5f;
		//go.transform.position = rightOuterWall;
		//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.localScale = Vector3.one * 0.5f;
		//go.transform.position = rightOuterFloor;

		for (var i = 1; i < numberOfQuads; i++)
		{
			var randomY = Vector3.up * Random.Range(-0.5f, 0.5f);
			leftOuterFloor = fromExit.BottomLeftExit + (thicknessDirection * 1) + (bottomLeftDirection * (bottomLeftQuadSize * i));
			leftOuterWall = fromExit.TopLeftExit + (thicknessDirection * 1) + (topLeftDirection * (topLeftQuadSize * i));
			leftWall = fromExit.TopLeftExit + (topLeftDirection * (topLeftQuadSize * i));
			leftFloor = fromExit.BottomLeftExit + (bottomLeftDirection * (bottomLeftQuadSize * i));
			rightFloor = fromExit.BottomRightExit + (bottomRightDirection * (bottomRightQuadSize * i));
			rightWall = fromExit.TopRightExit + (topRightDirection * (topRightQuadSize * i));
			rightOuterWall = fromExit.TopRightExit - (thicknessDirection * 1) + (topRightDirection * (topRightQuadSize * i));
			rightOuterFloor = fromExit.BottomRightExit - (thicknessDirection * 1) + (bottomRightDirection * (bottomRightQuadSize * i));

			vertices.Add(leftOuterFloor + randomY);
			vertices.Add(leftOuterWall + randomY);
			vertices.Add(leftWall + randomY);
			vertices.Add(leftFloor + randomY);
			vertices.Add(rightFloor + randomY);
			vertices.Add(rightWall + randomY);
			vertices.Add(rightOuterWall + randomY);
			vertices.Add(rightOuterFloor + randomY);

			//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.localScale = Vector3.one * 0.5f;
			//go.transform.position = leftOuterFloor;
			//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.localScale = Vector3.one * 0.5f;
			//go.transform.position = leftOuterWall;
			//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.localScale = Vector3.one * 0.5f;
			//go.transform.position = leftWall;
			//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.localScale = Vector3.one * 0.5f;
			//go.transform.position = leftFloor;
			//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.localScale = Vector3.one * 0.5f;
			//go.transform.position = rightFloor;
			//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.localScale = Vector3.one * 0.5f;
			//go.transform.position = rightWall;
			//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.localScale = Vector3.one * 0.5f;
			//go.transform.position = rightOuterWall;
			//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//go.transform.localScale = Vector3.one * 0.5f;
			//go.transform.position = rightOuterFloor;
		}

		leftOuterFloor = toExit.BottomRightExit;
		leftOuterWall = toExit.TopRightExit;
		leftWall = toExit.TopRightExit;
		leftFloor = toExit.BottomRightExit;
		rightFloor = toExit.BottomLeftExit;
		rightWall = toExit.TopLeftExit;
		rightOuterWall = toExit.TopLeftExit;
		rightOuterFloor = toExit.BottomLeftExit;

		vertices.Add(leftOuterFloor);
		vertices.Add(leftOuterWall);
		vertices.Add(leftWall);
		vertices.Add(leftFloor);
		vertices.Add(rightFloor);
		vertices.Add(rightWall);
		vertices.Add(rightOuterWall);
		vertices.Add(rightOuterFloor);

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

		corridor.SetVertices(newVertices);

		CompleteGameObject(newVertices, triangles);

		return triangles;
	}
}