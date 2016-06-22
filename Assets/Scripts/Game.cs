using UnityEngine;

public class Game : MonoBehaviour
{
	public bool Horror;
	public ModuleHandler ModuleHandler;
	public DungeonHandler DungeonHandler;
	public GameObject ThirdPersonDudePrefab;
	public GameObject FirstPersonDudePrefab;

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
		GameObject dude = null;
		if (Horror)
		{
			dude = Instantiate(FirstPersonDudePrefab);
		}
		else
		{
			dude = Instantiate(ThirdPersonDudePrefab);
		}

		dude.transform.position = DungeonHandler.StartingRoom.transform.position;
		PlayerTransform = dude.transform;
	}
}