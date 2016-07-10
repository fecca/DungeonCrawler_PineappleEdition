using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory
{
	private readonly RoomFactory _roomFactory = new RoomFactory();
	private readonly CorridorFactory _corridorFactory = new CorridorFactory();
	private readonly List<Bounds> _bounds = new List<Bounds>();
	private readonly List<Room> _rooms = new List<Room>();
	private readonly List<Corridor> _corridors = new List<Corridor>();

	private WorldHandler _worldHandler;
	private int _numberOfRoomsCreated;
	private int _numberOfRetries;

	public void CreateDungeon(WorldHandler worldHandler)
	{
		_worldHandler = worldHandler;

		// First room
		var currentRoom = CreateRoom(Vector3.zero);
		if (IsIntersecting(currentRoom))
		{
			return;
		}
		AddRoom(currentRoom);

		while (_numberOfRoomsCreated < Values.MaximumNumberOfRoomsAllowed && _numberOfRetries < Values.MaximumNumberOfRetries)
		{
			for (var i = 0; i < currentRoom.NumberOfExits; i++)
			{
				var currentExit = currentRoom.PotentialExits.GetRandomElement();

				// Raycast from random exit
				var raycastDistance = 50;
				var currentExitDirection = (currentExit.Position - currentRoom.Position).normalized;
				var corridorWidth = Vector3.Distance(currentExit.BottomLeftExit, currentExit.BottomRightExit);
				var ray = new Ray(currentExit.Position, currentExitDirection + Values.RandomRoomHeightPosition);
				if (Physics.SphereCast(ray, corridorWidth * 0.5f, raycastDistance))
				{
					_numberOfRetries++;
					continue;
				}

				// Create room with vertices and check for collision
				var newRoom = CreateRoom(ray.GetPoint(raycastDistance));
				if (IsIntersecting(newRoom))
				{
					_numberOfRetries++;
					continue;
				}

				// Setup exits
				var newExit = FindNearestExit(newRoom.PotentialExits, currentExit);
				currentRoom.Exit = currentExit;
				currentRoom.LinksTo = newExit;
				currentRoom.ExitCorners.Add(currentExit.CornerIndex);
				newRoom.ExitCorners.Add(newExit.CornerIndex);

				var corridor = CreateCorridor(currentExit, newExit);
				AddCorridor(corridor, corridorWidth);

				AddRoom(newRoom);
				currentRoom = newRoom;

				break;
			}
		}

		_worldHandler.DestroyColliders();

		CreateMeshes();
	}
	public void CreateMeshes()
	{
		for (var i = 0; i < _rooms.Count; i++)
		{
			_roomFactory.CreateTriangles(_rooms[i]);
		}

		for (var i = 0; i < _corridors.Count; i++)
		{
			_corridorFactory.CreateTriangles(_corridors[i]);
		}
	}

	private Room CreateRoom(Vector3 position)
	{
		var room = new Room(position, Values.RandomNumberOfRoomCorners, Values.RandomRoomRadius, Values.RandomRoomHeight, Values.RandomRoomThickness, Values.RandomNumberOfRoomExits);
		room.SetVertices(_roomFactory.CreateVertices(room));
		room.FindPotentialCorners();

		return room;
	}
	private void AddRoom(Room room)
	{
		AddTemporaryCollider(room.Position, room.Bounds.size, Vector3.zero);
		_bounds.Add(room.Bounds);
		_rooms.Add(room);
		_numberOfRoomsCreated++;
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
	private Corridor CreateCorridor(Exit from, Exit to)
	{
		var numberOfQuads = Values.RandomNumberOfCorridorQuads;
		var corridor = new Corridor(from, to, numberOfQuads);
		corridor.SetVertices(_corridorFactory.CreateVertices(from, to, numberOfQuads));

		return corridor;
	}
	private void AddCorridor(Corridor corridor, float corridorWidth)
	{
		var colliderSize = new Vector3(corridorWidth, corridorWidth, Vector3.Distance(corridor.From.Position, corridor.To.Position));
		AddTemporaryCollider(Vector3.Lerp(corridor.From.Position, corridor.To.Position, 0.5f), colliderSize, corridor.To.Position);
		_corridors.Add(corridor);
	}
	private void AddTemporaryCollider(Vector3 position, Vector3 size, Vector3 lookAtPosition)
	{
		var tmpGameObject = new GameObject();
		tmpGameObject.transform.position = position;

		var collider = tmpGameObject.AddComponent<BoxCollider>();
		collider.size = size;
		collider.transform.LookAt(lookAtPosition);

		_worldHandler.AddTemporaryCollider(collider);
	}
	private bool IsIntersecting(Room room)
	{
		for (var i = 0; i < _bounds.Count; i++)
		{
			if (_bounds[i].Intersects(room.Bounds))
			{
				return true;
			}
		}
		return false;
	}
}