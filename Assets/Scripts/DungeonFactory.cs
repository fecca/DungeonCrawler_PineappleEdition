using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory
{
	private RoomFactory _roomFactory = new RoomFactory();
	private CorridorFactory _corridorFactory = new CorridorFactory();

	public List<Module> CreateDungeon()
	{
		var roomList = new List<Module>();
		for (var i = 0; i < 1; i++)
		{
			var room = _roomFactory.CreateCircularRoom(Vector3.zero, 19, 10, 3, 1);
			roomList.Add(room);
		}

		for (var i = 0; i < roomList.Count; i++)
		{
			var room = roomList[i];
			room.CreateMesh();
		}

		//var corridor = _corridorFactory.CreateCorridor(room, 25);

		return new List<Module>()
		{
		};
	}
}