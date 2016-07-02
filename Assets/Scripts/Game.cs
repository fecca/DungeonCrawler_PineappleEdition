using UnityEngine;

public enum RoomArea
{
	None,
	Floor,
	Wall,
	All
}
public class Game : MonoBehaviour
{
	public GameObject Dude = null;
	public RoomArea SharedVertices = RoomArea.None;

	public static Transform PlayerTransform { get; set; }

	private void Start()
	{
		var dungeon = DungeonFactory.Instance.CreateDungeon(SharedVertices);
		CreateDude();
	}

	private void CreateDude()
	{
		GameObject dude = Instantiate(Dude);
		dude.transform.position = Vector3.zero;
	}
}