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
				var corridorWidth = Vector3.Distance(currentExit.BottomLeftExit, currentExit.BottomRightExit);
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

				// Craete corridor
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