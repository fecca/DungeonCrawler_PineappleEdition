using System.Collections.Generic;
using UnityEngine;

public class RoomFactory : ModuleFactory
{
	public GameObject CreateRoom(Room room)
	{
		var vertices = GenerateVertices(room.Position, room.NumberOfCorners, room.Radius, room.Height, room.Thickness);
		var exitCorners = FindExits(vertices, room.NumberOfExits);
		var triangles = GenerateTriangles(room, exitCorners, ref vertices);

		return CompleteGameObject(room.Position, vertices, triangles);
	}

	private List<Vector3> GenerateVertices(Vector3 position, int numberOfCorners, int radius, int height, int thickness)
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
			var wallVertex = cornerVertex + (Vector3.up * height);// + (directionNormalized * Random.Range(WallModifierIntervalMin, WallModifierIntervalMax));
			vertices.Add(wallVertex);

			// Outside wall vertex
			var outsideWallVertex = wallVertex + (directionNormalized * thickness);//(directionNormalized * Random.Range(WallModifierIntervalMin, WallModifierIntervalMax));
			vertices.Add(outsideWallVertex);

			// Outside corner vertex
			var outsideCornerVertex = directionNormalized + cornerVertex;
			vertices.Add(outsideCornerVertex);

			angle += 360f / numberOfCorners;
		}

		return vertices;
	}
	private List<int> FindExits(List<Vector3> vertices, int numberOfExits)
	{
		var tmpList = new List<int>();
		for (var i = 0; i < numberOfExits; i++)
		{
			tmpList.Add(i * 4);
		}
		return tmpList;
	}
	private List<int> GenerateTriangles(Room roomModel, List<int> exitCorners, ref List<Vector3> vertices)
	{
		var numberOfVerticesPerGroup = vertices.Count / roomModel.NumberOfCorners;

		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();
		for (var i = 0; i < roomModel.NumberOfCorners; i++)
		{
			var thisGroup = (numberOfVerticesPerGroup * i);
			var nextGroup = thisGroup + numberOfVerticesPerGroup >= vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

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

			// Floor
			{
				newVertices.Add(vertices[thisOrigoVertex]);
				newVertices.Add(vertices[thisFloorVertex1]);
				newVertices.Add(vertices[nextFloorVertex1]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[thisFloorVertex1]);
				newVertices.Add(vertices[thisFloorVertex2]);
				newVertices.Add(vertices[nextFloorVertex2]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextFloorVertex2]);
				newVertices.Add(vertices[nextFloorVertex1]);
				newVertices.Add(vertices[thisFloorVertex1]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[thisCornerVertex]);
				newVertices.Add(vertices[nextCornerVertex]);
				newVertices.Add(vertices[nextFloorVertex2]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextFloorVertex2]);
				newVertices.Add(vertices[thisFloorVertex2]);
				newVertices.Add(vertices[thisCornerVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			if (exitCorners.Contains(i))
			{
				// Save exit vertices
				var tmpList = new List<Vector3>();
				tmpList.Add(vertices[thisOutsideWallVertex]);
				tmpList.Add(vertices[thisOutsideCornerVertex]);
				tmpList.Add(vertices[nextOutsideCornerVertex]);
				tmpList.Add(vertices[nextOutsideWallVertex]);
				roomModel.ExitVertices.Add(tmpList);

				// Left wall
				{
					newVertices.Add(vertices[thisCornerVertex]);
					newVertices.Add(vertices[thisWallVertex]);
					newVertices.Add(vertices[thisOutsideWallVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);

					newVertices.Add(vertices[thisOutsideWallVertex]);
					newVertices.Add(vertices[thisOutsideCornerVertex]);
					newVertices.Add(vertices[thisCornerVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);
				}

				// Floor
				{
					newVertices.Add(vertices[thisCornerVertex]);
					newVertices.Add(vertices[thisOutsideCornerVertex]);
					newVertices.Add(vertices[nextOutsideCornerVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);

					newVertices.Add(vertices[nextOutsideCornerVertex]);
					newVertices.Add(vertices[nextCornerVertex]);
					newVertices.Add(vertices[thisCornerVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);
				}

				// Right wall
				{
					newVertices.Add(vertices[nextCornerVertex]);
					newVertices.Add(vertices[nextOutsideCornerVertex]);
					newVertices.Add(vertices[nextOutsideWallVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);

					newVertices.Add(vertices[nextOutsideWallVertex]);
					newVertices.Add(vertices[nextWallVertex]);
					newVertices.Add(vertices[nextCornerVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);
				}

				continue;
			}

			// Inside wall
			{
				newVertices.Add(vertices[thisCornerVertex]);
				newVertices.Add(vertices[thisWallVertex]);
				newVertices.Add(vertices[nextWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextWallVertex]);
				newVertices.Add(vertices[nextCornerVertex]);
				newVertices.Add(vertices[thisCornerVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			// Roof
			{
				newVertices.Add(vertices[thisWallVertex]);
				newVertices.Add(vertices[thisOutsideWallVertex]);
				newVertices.Add(vertices[nextOutsideWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextOutsideWallVertex]);
				newVertices.Add(vertices[nextWallVertex]);
				newVertices.Add(vertices[thisWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			// Outside wall
			{
				newVertices.Add(vertices[thisOutsideWallVertex]);
				newVertices.Add(vertices[thisOutsideCornerVertex]);
				newVertices.Add(vertices[nextOutsideCornerVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextOutsideCornerVertex]);
				newVertices.Add(vertices[nextOutsideWallVertex]);
				newVertices.Add(vertices[thisOutsideWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}
		}

		vertices = newVertices;

		return newTriangles;
	}
}