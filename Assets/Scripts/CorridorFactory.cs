using System.Collections.Generic;
using UnityEngine;

public class CorridorFactory : ModuleFactory
{
	private int _numberOfCorridorQuads = 25;
	public Module CreateCorridor(Module room)
	{
		// Clear data
		var vertices = new List<Vector3>();
		var triangles = new List<int>();
		var exitVertices = new List<Vector3>();

		var roomExitsVertices = room.GetExitVertices();

		var direction = (Vector3.Lerp(roomExitsVertices[1], roomExitsVertices[2], 0.5f) - room.transform.position).normalized;

		GenerateVertices(direction, roomExitsVertices, ref vertices);
		GenerateTrianglesWithUniqueVertices(ref vertices, ref triangles);

		return CompleteGameObject(vertices, triangles, exitVertices);
	}

	private void GenerateVertices(Vector3 direction, List<Vector3> roomExitsVertices, ref List<Vector3> vertices)
	{
		var leftPoint = roomExitsVertices[1];
		var rightPoint = roomExitsVertices[2];
		vertices.Add(leftPoint);
		vertices.Add(rightPoint);

		var corridorWidth = Vector3.Distance(leftPoint, rightPoint);

		for (var i = 0; i < _numberOfCorridorQuads; i++)
		{
			var randomYDirection = GetRandomDirectionY(direction.y);
			var randomLeftDirection = new Vector3(GetRandomDirectionX(direction.x), randomYDirection, GetRandomDirectionZ(direction.z));
			var randomRightDirection = new Vector3(GetRandomDirectionX(direction.x), randomYDirection, GetRandomDirectionZ(direction.z));

			var nextLeftPoint = leftPoint + (corridorWidth * randomLeftDirection);
			var nextRightPoint = rightPoint + (corridorWidth * randomRightDirection);

			vertices.Add(nextLeftPoint);
			vertices.Add(nextRightPoint);

			leftPoint = nextLeftPoint;
			rightPoint = nextRightPoint;

		}
	}
	private void GenerateTrianglesWithUniqueVertices(ref List<Vector3> vertices, ref List<int> triangles)
	{
		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();

		var index = 0;
		for (var i = 0; i < _numberOfCorridorQuads; i++)
		{
			// Floor
			{
				newVertices.Add(vertices[index + 0]);
				newVertices.Add(vertices[index + 2]);
				newVertices.Add(vertices[index + 3]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[index + 3]);
				newVertices.Add(vertices[index + 1]);
				newVertices.Add(vertices[index + 0]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}
			index += 2;

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