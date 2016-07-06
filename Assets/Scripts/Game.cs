using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public WorldHandler WorldHandler;
	public GameObject DudePrefab;

	private List<Room> _dungeon;
	private DungeonFactory _dungeonFactory;

	private void Awake()
	{
		_dungeonFactory = new DungeonFactory();
	}

	private void Start()
	{
		StartCoroutine(_dungeonFactory.Create(WorldHandler));
	}
}

public class RoomScript : MonoBehaviour
{
	public Room room;

	public void OnDrawGizmos()
	{
		if (room == null)
		{
			return;
		}

		Gizmos.DrawWireCube(room.Position, room.Bounds.size);
	}
}