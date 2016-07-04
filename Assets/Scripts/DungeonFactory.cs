using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory
{
	private RoomFactory _roomFactory = new RoomFactory();
	private CorridorFactory _corridorFactory = new CorridorFactory();

	public List<GameObject> CreateDungeon()
	{
		var roomList = new List<RoomModel>();
		for (var i = 0; i < 1; i++)
		{
			var roomModel = new RoomModel(Vector3.zero, 20, 10, 3, 1);
			while (IsIntersectingWithWorld(roomModel))
			{
				roomModel = new RoomModel(Vector3.one, 20, 10, 3, 1);
			}
			roomList.Add(roomModel);
		}

		var returnList = new List<GameObject>();
		for (var i = 0; i < roomList.Count; i++)
		{
			var roomModel = roomList[i];
			returnList.Add(_roomFactory.CreateRoom(roomModel));
			returnList.Add(_corridorFactory.CreateCorridor(roomModel, 8));
		}

		return returnList;
	}

	private bool IsIntersectingWithWorld(RoomModel roomModel)
	{
		return false;
	}
}