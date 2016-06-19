using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
	public ModuleHandler ModuleHandler;
	public DungeonHandler DungeonHandler;
	public GameObject DudePrefab;

	public static Transform PlayerTransform { get; set; }

	private void Start()
	{
		DungeonHandler.CreateDungeon(ModuleHandler);
		CreateDude();
	}

	private IEnumerator CreateDungeons()
	{
		for (var i = 0; i < 10; i++)
		{
			DungeonHandler.CreateDungeon(ModuleHandler);

			yield return new WaitForSeconds(5);
		}
	}
	private void CreateDude()
	{
		var dude = Instantiate(DudePrefab);
		dude.transform.position = DungeonHandler.StartingRoom.transform.position;

		PlayerTransform = dude.transform;
	}
}