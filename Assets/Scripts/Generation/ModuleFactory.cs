using System.Collections.Generic;
using UnityEngine;

public class ModuleFactory
{
	public List<DungeonObject> GenerateDungeonData(WorldHandler worldHandler)
	{
		var modules = new List<DungeonObject>();
		var numberOfRetries = 0;
		var numberOfRoomsCreated = 0;

		var currentRoom = CreateRoom(Vector3.zero);
		worldHandler.AddTemporaryCollider(currentRoom.Position, currentRoom.Bounds.size, Vector3.zero);
		modules.Add(currentRoom);
		numberOfRoomsCreated++;

		while (numberOfRoomsCreated < Values.MaximumNumberOfRoomsAllowed && numberOfRetries < Values.MaximumNumberOfRetries)
		{
			for (var i = 0; i < currentRoom.NumberOfExits; i++)
			{
				var currentExit = currentRoom.Corners.GetRandomElement();

				// Check corridor collision
				var raycastDistance = 50;
				var currentExitDirection = (currentExit.Position - currentRoom.Position).normalized;
				var corridorWidth = Vector3.Distance(currentExit.LeftFloor, currentExit.RightFloor);
				var ray = new Ray(currentExit.Position, currentExitDirection + Values.RandomRoomHeightPosition);
				if (Physics.SphereCast(ray, corridorWidth * 0.5f, raycastDistance))
				{
					numberOfRetries++;
					continue;
				}

				// Check room collision
				var newRoomPosition = ray.GetPoint(raycastDistance);
				if (Physics.CheckSphere(newRoomPosition, Values.RoomRadiusMax))
				{
					continue;
				}

				// Create room
				var newRoom = CreateRoom(newRoomPosition);
				worldHandler.AddTemporaryCollider(newRoom.Position, newRoom.Bounds.size, Vector3.zero);
				modules.Add(newRoom);
				numberOfRoomsCreated++;

				// Set exits
				var newExit = FindNearestExit(newRoom.Corners, currentExit);
				currentRoom.Exit = currentExit;
				currentRoom.LinksTo = newExit;
				currentRoom.ExitCorners.Add(currentExit.CornerIndex);
				newRoom.ExitCorners.Add(newExit.CornerIndex);

				// Create corridor
				var corridor = CreateCorridor(currentExit, newExit);
				modules.Add(corridor);
				var colliderSize = new Vector3(corridorWidth, corridorWidth, Vector3.Distance(corridor.From.Position, corridor.To.Position));
				worldHandler.AddTemporaryCollider(Vector3.Lerp(corridor.From.Position, corridor.To.Position, 0.5f), colliderSize, corridor.To.Position);

				currentRoom = newRoom;

				break;
			}
		}

		return modules;
	}

	private Room CreateRoom(Vector3 position)
	{
		var room = new Room(position, Values.RandomNumberOfRoomCorners, Values.RandomRoomRadius, Values.RandomRoomHeight, Values.RandomRoomThickness, Values.RandomNumberOfRoomExits);
		var vertices = CreateRoomVertices(room);
		room.SetVertices(vertices);

		return room;
	}
	private List<Vector3> CreateRoomVertices(Room room)
	{
		var vertices = new List<Vector3>();
		var angle = Random.Range(0f, 360f);
		var angleStep = 360f / room.NumberOfCorners;
		for (var i = 0; i < room.NumberOfCorners; i++)
		{
			var roomPosition = room.Position;
			var modifiedAngle = angle + (angleStep * 0.5f) * Values.RandomRoomCornerAngle;
			var cornerRadius = room.Radius * Values.RandomRoomRadiusInterval;
			var x = Mathf.Sin(Mathf.Deg2Rad * modifiedAngle) * cornerRadius;
			var z = Mathf.Cos(Mathf.Deg2Rad * modifiedAngle) * cornerRadius;

			// Origo
			vertices.Add(roomPosition);

			// Floor vertex 1
			var floorVertex1 = new Vector3(roomPosition.x + x * 0.33f, roomPosition.y + Values.RandomRoomFloorHeight, roomPosition.z + z * 0.33f);
			vertices.Add(floorVertex1);

			// Floor vertex 2
			var floorVertex2 = new Vector3(roomPosition.x + x * 0.66f, roomPosition.y + Values.RandomRoomFloorHeight, roomPosition.z + z * 0.66f);
			vertices.Add(floorVertex2);

			// Corner vertex
			var cornerVertex = new Vector3(roomPosition.x + x, roomPosition.y, roomPosition.z + z);
			vertices.Add(cornerVertex);

			var directionNormalized = (cornerVertex - roomPosition).normalized;

			// Wall vertex
			var wallVertex = cornerVertex + (Vector3.up * room.Height);
			vertices.Add(wallVertex);

			// Outside wall vertex
			var outsideWallVertex = wallVertex + (directionNormalized * room.Thickness);
			vertices.Add(outsideWallVertex);

			// Outside corner vertex
			var outsideCornerVertex = directionNormalized + cornerVertex;
			vertices.Add(outsideCornerVertex);

			angle += 360f / room.NumberOfCorners;
		}

		return vertices;
	}
	private Corridor CreateCorridor(Exit from, Exit to)
	{
		var numberOfQuads = Values.RandomNumberOfCorridorQuads;
		var corridor = new Corridor(from, to, numberOfQuads);
		var vertices = CreateCorridorVertices(from, to, numberOfQuads);
		corridor.SetVertices(vertices);

		return corridor;
	}
	private List<Vector3> CreateCorridorVertices(Exit fromExit, Exit toExit, int numberOfQuads)
	{
		var vertices = new List<Vector3>();

		var leftOuterFloorDistance = Vector3.Distance(fromExit.LeftOuterFloor, toExit.RightOuterFloor);
		var leftOuterFloorDirection = (toExit.RightOuterFloor - fromExit.LeftOuterFloor).normalized;
		var leftOuterFloorQuadLength = leftOuterFloorDistance / numberOfQuads;
		vertices.Add(fromExit.LeftOuterFloor);

		var leftOuterWallDistance = Vector3.Distance(fromExit.LeftOuterWall, toExit.RightOuterWall);
		var leftOuterWallDirection = (toExit.RightOuterWall - fromExit.LeftOuterWall).normalized;
		var leftOuterWallQuadLength = leftOuterWallDistance / numberOfQuads;
		vertices.Add(fromExit.LeftOuterWall);

		var leftWallDistance = Vector3.Distance(fromExit.LeftWall, toExit.RightWall);
		var leftWallDirection = (toExit.RightWall - fromExit.LeftWall).normalized;
		var leftWallQuadLength = leftWallDistance / numberOfQuads;
		vertices.Add(fromExit.LeftWall);

		var leftFloorDistance = Vector3.Distance(fromExit.LeftFloor, toExit.RightFloor);
		var leftFloorDirection = (toExit.RightFloor - fromExit.LeftFloor).normalized;
		var leftFloorQuadLength = leftFloorDistance / numberOfQuads;
		vertices.Add(fromExit.LeftFloor);

		var rightFloorDistance = Vector3.Distance(fromExit.RightFloor, toExit.LeftFloor);
		var rightFloorDirection = (toExit.LeftFloor - fromExit.RightFloor).normalized;
		var rightFloorQuadLength = rightFloorDistance / numberOfQuads;
		vertices.Add(fromExit.RightFloor);

		var rightWallDistance = Vector3.Distance(fromExit.RightWall, toExit.LeftWall);
		var rightWallDirection = (toExit.LeftWall - fromExit.RightWall).normalized;
		var rightWallQuadLength = rightWallDistance / numberOfQuads;
		vertices.Add(fromExit.RightWall);

		var rightOuterWallDistance = Vector3.Distance(fromExit.RightOuterWall, toExit.LeftOuterWall);
		var rightOuterWallDirection = (toExit.LeftOuterWall - fromExit.RightOuterWall).normalized;
		var rightOuterWallQuadLength = rightOuterWallDistance / numberOfQuads;
		vertices.Add(fromExit.RightOuterWall);

		var rightOuterFloorDistance = Vector3.Distance(fromExit.RightOuterFloor, toExit.LeftOuterFloor);
		var rightOuterFloorDirection = (toExit.LeftOuterFloor - fromExit.RightOuterFloor).normalized;
		var rightOuterFloorQuadLength = rightOuterFloorDistance / numberOfQuads;
		vertices.Add(fromExit.RightOuterFloor);

		for (var i = 1; i < numberOfQuads; i++)
		{
			var leftOuterFloor = fromExit.LeftOuterFloor + leftOuterFloorDirection * leftOuterFloorQuadLength * i;
			var leftOuterWall = fromExit.LeftOuterWall + leftOuterWallDirection * leftOuterWallQuadLength * i;
			var leftWall = fromExit.LeftWall + leftWallDirection * leftWallQuadLength * i;
			var leftFloor = fromExit.LeftFloor + leftFloorDirection * leftFloorQuadLength * i;
			var rightFloor = fromExit.RightFloor + rightFloorDirection * rightFloorQuadLength * i;
			var rightWall = fromExit.RightWall + rightWallDirection * rightWallQuadLength * i;
			var rightOuterWall = fromExit.RightOuterWall + rightOuterWallDirection * rightOuterWallQuadLength * i;
			var rightOuterFloor = fromExit.RightOuterFloor + rightOuterFloorDirection * rightOuterFloorQuadLength * i;

			vertices.Add(Vector3.Lerp(leftOuterFloor, leftFloor, 0.5f));
			vertices.Add(Vector3.Lerp(leftOuterWall, leftWall, 0.5f));
			vertices.Add(leftWall);
			vertices.Add(leftFloor);
			vertices.Add(rightFloor);
			vertices.Add(rightWall);
			vertices.Add(Vector3.Lerp(rightOuterWall, rightWall, 0.5f));
			vertices.Add(Vector3.Lerp(rightOuterFloor, rightFloor, 0.5f));
		}

		vertices.Add(toExit.RightOuterFloor);
		vertices.Add(toExit.RightOuterWall);
		vertices.Add(toExit.RightWall);
		vertices.Add(toExit.RightFloor);
		vertices.Add(toExit.LeftFloor);
		vertices.Add(toExit.LeftWall);
		vertices.Add(toExit.LeftOuterWall);
		vertices.Add(toExit.LeftOuterFloor);

		return vertices;
	}
	private Exit FindNearestExit(List<Exit> exits, Exit referenceExit)
	{
		var nearest = exits[0];
		var distance = Vector3.Distance(nearest.Position, referenceExit.Position);
		for (var i = 0; i < exits.Count; i++)
		{
			var exit = exits[i];
			var tmpDistance = Vector3.Distance(exit.Position, referenceExit.Position);
			if (tmpDistance < distance)
			{
				nearest = exit;
				distance = tmpDistance;
			}
		}

		return nearest;
	}
}