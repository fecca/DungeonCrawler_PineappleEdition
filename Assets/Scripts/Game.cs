using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject DudePrefab = null;

	private List<GameObject> _dungeon;

	private void Start()
	{
		_dungeon = DungeonFactory.Instance.CreateDungeon();
		//CreateDude();
	}

	private void CreateDude()
	{
		GameObject dude = Instantiate(DudePrefab, Vector3.zero, DudePrefab.transform.rotation) as GameObject;
	}
}