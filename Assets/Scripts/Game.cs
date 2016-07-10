using UnityEngine;

public class Game : MonoBehaviour
{
	public WorldHandler WorldHandler;
	public GameObject DudePrefab;

	private DungeonFactory _dungeonFactory;

	private void Awake()
	{
		_dungeonFactory = new DungeonFactory();
	}

	private void Start()
	{
		_dungeonFactory.CreateDungeon(WorldHandler);
		Instantiate(DudePrefab, Vector3.up, Quaternion.identity);
	}
}