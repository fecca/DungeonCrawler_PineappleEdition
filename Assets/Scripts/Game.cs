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

		//DungeonHandler.CreateDungeon(ModuleHandler);

		//RoomFactory.Instance.CreateRoom(new Vector3(0, 0, 0), 3, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(25, 0, 0), 4, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(50, 0, 0), 5, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(75, 0, 0), 6, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(100, 0, 0), 7, 10, 1, 1);
		RoomFactory.Instance.CreateRoom(new Vector3(125, 0, 0), 8, 10, 1, 1);

		//RoomFactory.Instance.CreateRoom(new Vector3(0, 0, 20), 9, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(25, 0, 20), 8, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(50, 0, 20), 11, 10, 1, 1);
		RoomFactory.Instance.CreateRoom(new Vector3(75, 0, 20), 12, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(100, 0, 20), 13, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(125, 0, 20), 14, 10, 1, 1);

		//RoomFactory.Instance.CreateRoom(new Vector3(0, 0, 40), 15, 10, 1, 1);
		RoomFactory.Instance.CreateRoom(new Vector3(25, 0, 40), 16, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(50, 0, 40), 17, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(75, 0, 40), 18, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(100, 0, 40), 19, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(125, 0, 40), 20, 10, 1, 1);

		//RoomFactory.Instance.CreateRoom(new Vector3(0, 0, 60), 25, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(25, 0, 60), 30, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(50, 0, 60), 35, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(75, 0, 60), 40, 10, 1, 1);
		RoomFactory.Instance.CreateRoom(new Vector3(100, 0, 60), 45, 10, 1, 1);
		//RoomFactory.Instance.CreateRoom(new Vector3(125, 0, 60), 50, 10, 1, 1);
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

		dude.transform.position = DungeonHandler.StartingRoom.transform.position + Vector3.up;
		PlayerTransform = dude.transform;
	}
}