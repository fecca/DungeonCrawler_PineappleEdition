using UnityEngine;

public class Game : MonoBehaviour
{
	public ModuleHandler ModuleHandler;
	public DungeonHandler DungeonHandler;
	public GameObject DudePrefab;

	public static Transform PlayerTransform { get; set; }

	private void Start()
	{
		MessageHub.Instance.Subscribe<DungeonCreatedMessage>(OnDungeonCreated);

		DungeonHandler.CreateDungeon(ModuleHandler);
	}

	private void OnDungeonCreated(DungeonCreatedMessage message)
	{
		CreateDude();
	}
	private void CreateDude()
	{
		var dude = Instantiate(DudePrefab);
		dude.transform.position = DungeonHandler.StartingRoom.transform.position;

		PlayerTransform = dude.transform;
	}
}