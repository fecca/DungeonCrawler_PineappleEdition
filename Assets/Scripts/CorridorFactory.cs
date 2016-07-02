using UnityEngine;

public class CorridorFactory
{
	private static CorridorFactory _instance;
	public static CorridorFactory Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CorridorFactory();
			}
			return _instance;
		}
	}

	private CorridorFactory() { }

	public GameObject CreateCorridor(Room room)
	{
		var roomVertices = room.GetExitVertices();

		return new GameObject();
	}
}