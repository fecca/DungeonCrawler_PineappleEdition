using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject DudePrefab = null;

	private List<GameObject> _dungeon;
	private DungeonFactory _dungeonFactory = new DungeonFactory();

	private void Start()
	{
		_dungeon = _dungeonFactory.CreateDungeon();
	}
}