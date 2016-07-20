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
	public List<Exit> Corners { get; set; }
	public List<int> ExitCorners { get; set; }
	public Exit Exit { get; set; }
	public Exit LinksTo { get; set; }

	public List<Exit> LeadsTo { get; set; }

	public Room(Vector3 position, int numberOfCorners, int radius, int height, int thickness, int numberOfExits)
	{
		Type = ModuleType.Room;

		Position = position;
		NumberOfCorners = numberOfCorners;
		Radius = radius;
		Height = height;
		Thickness = thickness;
		NumberOfExits = numberOfExits;
		Bounds = new Bounds(Position, new Vector3(Radius * 2 + Thickness * 2, Height, Radius * 2 + Thickness * 2));
		ExitCorners = new List<int>();
		Corners = new List<Exit>();
	}

	public override void SetVertices(List<Vector3> vertices)
	{
		base.SetVertices(vertices);
		FindPotentialCorners();
	}

	private void FindPotentialCorners()
	{
		var groupSize = Vertices.Count / NumberOfCorners;

		for (var i = 0; i < NumberOfCorners; i++)
		{
			var thisGroup = i;
			var thisOuterGroup = thisGroup - 1 < 0 ? NumberOfCorners - 1 : thisGroup - 1;
			var nextGroup = thisGroup + 1 >= NumberOfCorners ? 0 : thisGroup + 1;
			var nextOuterGroup = nextGroup + 1 >= NumberOfCorners ? 0 : nextGroup + 1;

			Corners.Add(new Exit(i,
				Vertices[thisOuterGroup * groupSize + 6],
				Vertices[thisOuterGroup * groupSize + 5],
				Vertices[thisGroup * groupSize + 5],
				Vertices[thisGroup * groupSize + 6],
				Vertices[nextGroup * groupSize + 6],
				Vertices[nextGroup * groupSize + 5],
				Vertices[nextOuterGroup * groupSize + 5],
				Vertices[nextOuterGroup * groupSize + 6]));
		}
	}
}