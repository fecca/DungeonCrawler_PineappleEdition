public class Corridor : DungeonObject
{
	public Exit From { get; private set; }
	public Exit To { get; private set; }
	public int NumberOfQuads { get; set; }

	public Corridor(Exit from, Exit to, int numberOfQuads)
	{
		From = from;
		To = to;
		NumberOfQuads = numberOfQuads;
	}
}