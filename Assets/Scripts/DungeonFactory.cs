using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory
{
	private RoomFactory _roomFactory = new RoomFactory();
	private CorridorFactory _corridorFactory = new CorridorFactory();
	private WorldHandler _worldHandler;

	private int _numberOfRoomsCreated;
	private const int MaximumNumberOfRoomsAllowed = 100;
	private int _numberOfRetries;
	private const int MaximumNumberOfRetries = 50;
	private List<Bounds> _bounds = new List<Bounds>();
	private List<Room> _rooms = new List<Room>();

	private const float CorridorWidth = 5f;

	public IEnumerator Create(WorldHandler worldHandler)
	{
		_worldHandler = worldHandler;

		var room = CreateRoom(Vector3.zero, 10, 10, 4, 1, 3);
		if (IsIntersecting(room))
		{
			yield return null;
		}

		AddRoom(room);

		var go = new GameObject();
		var roomScript = go.AddComponent<RoomScript>();
		roomScript.room = room;

		while (_numberOfRoomsCreated < MaximumNumberOfRoomsAllowed && _numberOfRetries < MaximumNumberOfRetries)
		{
			for (var i = 0; i < room.NumberOfExits; i++)
			{
				var exit = room.Exits.GetRandomElement();

				var raycastDistance = 50;
				var ray = new Ray(exit.Position, exit.Direction);
				var endOfRayCast = ray.GetPoint(raycastDistance);

				// ToDo: skip raycast, draw bounds instead
				if (Physics.SphereCast(ray, CorridorWidth * 0.5f, raycastDistance))
				{
					Debug.Log("hit");
					_numberOfRetries++;
					continue;
				}

				var tmpRoom = CreateRoom(endOfRayCast, 10, 10, 5, 1, 3);
				if (IsIntersecting(tmpRoom))
				{
					Debug.Log("intersected");
					_numberOfRetries++;
					continue;
				}

				var corridorCenter = Vector3.Lerp(exit.Position, endOfRayCast, 0.5f);
				var corridorSize = new Vector3(CorridorWidth, CorridorWidth, raycastDistance);
				var corridorAngle = Vector3.Angle(exit.Position, endOfRayCast);
				AddCollider(corridorCenter, corridorSize, endOfRayCast);

				room = tmpRoom;
				AddRoom(tmpRoom);

				var roomExit = room.Exits[0].Position;

				// Debugging
				Debug.DrawRay(exit.Position, exit.Direction * raycastDistance, Color.red, 100f);
				go = new GameObject();
				roomScript = go.AddComponent<RoomScript>();
				roomScript.room = room;

				yield return new WaitForSeconds(0.1f);

				break;
			}
		}

		_worldHandler.DestroyColliders();
	}
	private void AddRoom(Room room)
	{
		AddCollider(room.Position, room.Bounds.size, Vector3.zero);

		_bounds.Add(room.Bounds);
		_rooms.Add(room);
		_numberOfRoomsCreated++;
	}
	private void AddCollider(Vector3 position, Vector3 size, Vector3 lookAtPosition)
	{
		var tmpGameObject = new GameObject();
		tmpGameObject.transform.position = position;

		var collider = tmpGameObject.AddComponent<BoxCollider>();
		collider.size = size;
		collider.transform.LookAt(lookAtPosition);

		_worldHandler.AddTemporaryCollider(tmpGameObject);
	}
	private Room CreateRoom(Vector3 position, int numberOfCorners, int radius, int height, int thickness, int numberOfExits)
	{
		var room = new Room(position, numberOfCorners, radius, height, thickness, numberOfExits);
		var vertices = _roomFactory.CreateVertices(room);
		room.SetVertices(vertices);

		return room;
	}










	public List<Room> CreateDungeon()
	{
		//CreateShell();

		return _rooms;
	}
	private void CreateShell()
	{
		var room = new Room(Vector3.zero, 10, 10, 4, 1, 3);
		var vertices = _roomFactory.CreateVertices(room);
		room.SetVertices(vertices);

		if (IsIntersecting(room))
		{
			return;
		}

		_bounds.Add(room.Bounds);
		_rooms.Add(room);

		while (_numberOfRoomsCreated < MaximumNumberOfRoomsAllowed)
		{
			var exits = room.Exits;

			for (var i = 0; i < room.NumberOfExits; i++)
			{
				var exit = exits.GetRandomElement();

				var raycastDistance = room.Radius * 5;
				var ray = new Ray(exit.Position, exit.Direction);
				if (Physics.SphereCast(ray, CorridorWidth, raycastDistance))
				{
					continue;
				}

				var endOfCorridor = ray.GetPoint(raycastDistance);

				Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 100f);
				//_bounds.Add(new Bounds(Vector3.Lerp(ray.origin, endOfCorridor, 0.5f), new Vector3(raycastDistance, CorridorRadius, CorridorRadius)));

				var roomRadius = 10;
				var newRoomPosition = endOfCorridor + (ray.direction * room.Radius);
				room = new Room(newRoomPosition, 10, roomRadius, 4, 1, 3);
				var tmpVertices = _roomFactory.CreateVertices(room);
				room.SetVertices(tmpVertices);

				if (IsIntersecting(room))
				{
					continue;
				}

				_bounds.Add(room.Bounds);
				_rooms.Add(room);
				_numberOfRoomsCreated++;

				break;
			}
		}
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

	//private void CreateRoomData()
	//{
	//	for (var i = 0; i < 2; i++)
	//	{
	//		var randomPosition = new Vector3(Random.Range(-200, 250), 0, Random.Range(-250, 250));
	//		var room = new Room(randomPosition, 20, 10, 3, 1, 4);
	//		while (IsIntersectingWithWorld(room))
	//		{
	//			randomPosition = new Vector3(Random.Range(-200, 250), 0, Random.Range(-250, 250));
	//			room = new Room(randomPosition, 20, 10, 3, 1, 4);
	//		}
	//		_roomList.Add(room);
	//	}
	//}
	//private List<GameObject> CreateRoomsAndCorridors()
	//{
	//	var gameObjects = new List<GameObject>();
	//	for (var i = 0; i < _roomList.Count; i++)
	//	{
	//		var room = _roomList[i];
	//		gameObjects.Add(_roomFactory.CreateRoom(room));
	//		for (var j = 0; j < room.ExitVertices.Count; j++)
	//		{
	//			var exitVertices = room.ExitVertices[j];
	//			gameObjects.Add(_corridorFactory.CreateCorridor(room.Position, exitVertices, 8));
	//		}
	//	}

	//	return gameObjects;
	//}
}