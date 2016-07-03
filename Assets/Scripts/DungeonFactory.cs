using System.Collections.Generic;

public class DungeonFactory
{
	private RoomFactory _roomFactory = new RoomFactory();
	private CorridorFactory _corridorFactory = new CorridorFactory();

	public List<Module> CreateDungeon()
	{
		var room = _roomFactory.CreateRoom(null, 19, 10, 1, 3);
		var corridor = _corridorFactory.CreateCorridor(room);

		return new List<Module>()
		{
			room,
			corridor
		};
	}
}