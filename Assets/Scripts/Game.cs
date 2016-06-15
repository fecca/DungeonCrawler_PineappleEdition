using UnityEngine;

public class Game : MonoBehaviour
{
	public DungeonHandler DungeonHandler;
	public GameObject DudePrefab;

	private void Start()
	{
		DungeonHandler.CreateDungeon();

		CreateDude();
	}

	private void CreateDude()
	{
		var dude = Instantiate(DudePrefab);
		dude.transform.position = DungeonHandler.GetStartingPosition();
	}
}