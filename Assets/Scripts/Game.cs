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
		var dungeon = DungeonFactory.Instance.CreateDungeon();
		CreateDude();
	}

	private void CreateDude()
	{
		GameObject dude = Instantiate(ThirdPersonDudePrefab);
		dude.transform.position = Vector3.zero;
	}
}