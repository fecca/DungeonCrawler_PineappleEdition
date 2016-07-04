using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory
{
	private RoomFactory _roomFactory = new RoomFactory();
	private CorridorFactory _corridorFactory = new CorridorFactory();
	private List<Room> _roomList = new List<Room>();

	public List<GameObject> CreateDungeon()
	{
		CreateRoomData();
		var dungeonObjects = CreateRoomsAndCorridors();

		return dungeonObjects;
	}

	private void CreateRoomData()
	{
		for (var i = 0; i < 2; i++)
		{
			var randomPosition = new Vector3(Random.Range(-200, 250), 0, Random.Range(-250, 250));
			var room = new Room(randomPosition, 20, 10, 3, 1, 4);
			while (IsIntersectingWithWorld(room))
			{
				randomPosition = new Vector3(Random.Range(-200, 250), 0, Random.Range(-250, 250));
				room = new Room(randomPosition, 20, 10, 3, 1, 4);
			}
			_roomList.Add(room);
		}
	}
	private List<GameObject> CreateRoomsAndCorridors()
	{
		var gameObjects = new List<GameObject>();
		for (var i = 0; i < _roomList.Count; i++)
		{
			var room = _roomList[i];
			gameObjects.Add(_roomFactory.CreateRoom(room));
			for (var j = 0; j < room.ExitVertices.Count; j++)
			{
				var exitVertices = room.ExitVertices[j];
				gameObjects.Add(_corridorFactory.CreateCorridor(room.Position, exitVertices, 8));
			}
		}

		return gameObjects;
	}
	private bool IsIntersectingWithWorld(Room room)
	{
		for (var i = 0; i < _roomList.Count; i++)
		{
			var tmpRoom = _roomList[i];
			var minimumAllowedDistance = (tmpRoom.Radius + room.Radius) + 1;
			if (Vector3.Distance(tmpRoom.Position, room.Position) <= minimumAllowedDistance)
			{
				return true;
			}
		}
		return false;
	}
}