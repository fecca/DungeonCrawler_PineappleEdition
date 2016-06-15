using UnityEngine;

public class Game : MonoBehaviour
{
	public DungeonHandler DungeonHandler;

	private void Start()
	{
		DungeonHandler.CreateDungeon();
	}
}