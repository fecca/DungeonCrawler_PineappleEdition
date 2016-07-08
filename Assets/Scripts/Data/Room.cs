using UnityEngine;
using System.Collections.Generic;

public class Room : DungeonObject
{
	public Vector3 Position { get; private set; }
	public int NumberOfCorners { get; private set; }
	public int Radius { get; private set; }
	public int Height { get; private set; }
	public int Thickness { get; private set; }
	public int NumberOfExits { get; private set; }
	public Bounds Bounds { get; private set; }
	public List<Exit> PotentialExits { get; set; }
	public List<int> ExitCorners { get; set; }
	public Exit Exit { get; set; }
	public Exit LinksTo { get; set; }

	public Room(Vector3 position, int numberOfCorners, int radius, int height, int thickness, int numberOfExits)
	{
		Position = position;
		NumberOfCorners = numberOfCorners;
		Radius = radius;
		Height = height;
		Thickness = thickness;
		NumberOfExits = numberOfExits;
		Bounds = new Bounds(Position, new Vector3(Radius * 2 + Thickness * 2, Height, Radius * 2 + Thickness * 2));
		ExitCorners = new List<int>();
		PotentialExits = new List<Exit>();
	}

	public void FindPotentialCorners()
	{
		var groupSize = Vertices.Count / NumberOfCorners;

		for (var i = 0; i < NumberOfCorners; i++)
		{
			var thisGroup = groupSize * i;
			var nextGroup = i < NumberOfCorners - 1 ? thisGroup + groupSize : 0;
			var topLeft = Vertices[thisGroup + 5];
			var bottomLeft = Vertices[thisGroup + 6];
			var bottomRight = Vertices[nextGroup + 6];
			var topRight = Vertices[nextGroup + 5];

			PotentialExits.Add(new Exit(i, topLeft, bottomLeft, bottomRight, topRight));
		}
	}
}