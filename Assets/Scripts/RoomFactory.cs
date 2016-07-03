using System.Collections.Generic;
using UnityEngine;

public class RoomFactory : ModuleFactory
{
	public Room CreateCircularRoom(Vector3 position, int numberOfCorners, int radius, int height, int thickness)
	{
		var newGameObject = new GameObject("Module");
		newGameObject.transform.position = position;

		var vertices = GenerateVertices(position, numberOfCorners, radius, height, thickness);
		var roomModel = new RoomModel(numberOfCorners, radius, height, thickness)
		{
			SharedVertices = vertices,
			ExitCorners = new List<int>() { 0 }
		};

		var room = newGameObject.AddComponent<Room>();
		room.Model = roomModel;

		return room;
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
			var wallVertex = cornerVertex + (directionNormalized * Random.Range(WallModifierIntervalMin, WallModifierIntervalMax)) + (Vector3.up * height);
			vertices.Add(wallVertex);

			// Outside wall vertex
			var outsideWallVertex = wallVertex + directionNormalized + (directionNormalized * Random.Range(WallModifierIntervalMin, WallModifierIntervalMax));
			vertices.Add(outsideWallVertex);

			// Outside corner vertex
			var outsideCornerVertex = directionNormalized + cornerVertex;
			vertices.Add(outsideCornerVertex);

			angle += 360f / numberOfCorners;
		}

		return vertices;
	}
}