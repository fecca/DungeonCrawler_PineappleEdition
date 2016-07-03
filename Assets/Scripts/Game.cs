using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject DudePrefab = null;

	private List<Module> _dungeon;
	private DungeonFactory _dungeonFactory = new DungeonFactory();

	private void Start()
	{
		_dungeon = _dungeonFactory.CreateDungeon();
		//CreateDude();
	}

	private void CreateDude()
	{
		GameObject dude = Instantiate(DudePrefab, Vector3.zero, DudePrefab.transform.rotation) as GameObject;
	}
}