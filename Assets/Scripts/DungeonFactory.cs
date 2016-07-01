using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory
{
	private static DungeonFactory _instance;
	public static DungeonFactory Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DungeonFactory();
			}
			return _instance;
		}
	}

	private DungeonFactory() { }

	public List<GameObject> CreateDungeon()
	{
		var roomsList = new List<GameObject>()
		{
			RoomFactory.Instance.CreateRoom(new Vector3(0, 0, 0), 3, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(25, 0, 0), 5, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(50, 0, 0), 7, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(75, 0, 0), 9, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(100, 0, 0), 11, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(125, 0, 0), 13, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(150, 0, 0), 15, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(175, 0, 0), 17, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(200, 0, 0), 19, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(0, 0, 25), 21, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(25, 0, 25), 23, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(50, 0, 25), 25, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(75, 0, 25), 27, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(100, 0, 25), 29, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(125, 0, 25), 31, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(150, 0, 25), 33, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(175, 0, 25), 35, 10, 1, 4),
			RoomFactory.Instance.CreateRoom(new Vector3(200, 0, 25), 37, 10, 1, 4)
		};
		return roomsList;
	}
}