using UnityEngine;

public class Game : MonoBehaviour
{
	public WorldHandler WorldHandler;

	private DungeonFactory _dungeonFactory;

	private void Awake()
	{
		MessageHub.Instance.Subscribe<DungeonCreatedMessage>((message) => DungeonCreated());

		_dungeonFactory = new DungeonFactory();
	}

	private void Start()
	{
		StartCoroutine(_dungeonFactory.CreateDungeon(WorldHandler));
	}

	private void DungeonCreated()
	{
		StartCoroutine(_dungeonFactory.CreateMeshes());
	}
}