using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public WorldHandler WorldHandler;
	public GameObject DudePrefab;

	//private List<Room> _dungeon;
	private DungeonFactory _dungeonFactory;

	private void Awake()
	{
		_dungeonFactory = new DungeonFactory();
	}

	private void Start()
	{
		MessageHub.Instance.Subscribe<DungeonCreatedMessage>((message) => DungeonCreated());
		StartCoroutine(_dungeonFactory.CreateDungeon(WorldHandler));
	}

	private void DungeonCreated()
	{
		StartCoroutine(_dungeonFactory.CreateMeshes());
	}
}

//public class RoomScript : MonoBehaviour
//{
//	public Room Room { get; set; }

//	public void OnDrawGizmos()
//	{
//		if (Room == null)
//		{
//			return;
//		}

//		Gizmos.DrawWireSphere(Room.Position, Room.Radius);
//	}
//}