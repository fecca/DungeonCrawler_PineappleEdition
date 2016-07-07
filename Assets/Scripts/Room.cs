using UnityEngine;
using System.Collections.Generic;

public class Room
{
	public Vector3 Position { get; private set; }
	public int NumberOfCorners { get; private set; }
	public int Radius { get; private set; }
	public int Height { get; private set; }
	public int Thickness { get; private set; }
	public int NumberOfExits { get; private set; }
	public List<Vector3> Vertices { get; private set; }
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
		Vertices = new List<Vector3>();
		Bounds = new Bounds(Position, new Vector3(Radius * 2 + Thickness * 2, Height, Radius * 2 + Thickness * 2));
		ExitCorners = new List<int>();
	}

	public void SetVertices(List<Vector3> vertices)
	{
		Vertices = vertices;
	}
	public void FindPotentialCorners()
	{
		PotentialExits = FindPotentialExits();
	}

	private List<Exit> FindPotentialExits()
	{
		var groupSize = Vertices.Count / NumberOfCorners;

		var tmpList = new List<Exit>();
		for (var i = 0; i < NumberOfCorners; i++)
		{
			var thisGroup = groupSize * i;
			var nextGroup = i < NumberOfCorners - 1 ? thisGroup + groupSize : 0;
			var topLeft = Vertices[thisGroup + 5];
			var bottomLeft = Vertices[thisGroup + 6];
			var bottomRight = Vertices[nextGroup + 6];
			var topRight = Vertices[nextGroup + 5];

			tmpList.Add(new Exit(i, topLeft, bottomLeft, bottomRight, topRight));
		}

		return tmpList;
	}
}

public class Exit
{
	public int CornerIndex { get; private set; }
	public Vector3 TopLeftExit { get; private set; }
	public Vector3 BottomLeftExit { get; private set; }
	public Vector3 BottomRightExit { get; private set; }
	public Vector3 TopRightExit { get; private set; }
	public Vector3 Position { get; private set; }

	public Exit(int cornerIndex, Vector3 topLeftExit, Vector3 bottomLeftExit, Vector3 bottomRightExit, Vector3 topRightExit)
	{
		CornerIndex = cornerIndex;
		TopLeftExit = topLeftExit;
		BottomLeftExit = bottomLeftExit;
		BottomRightExit = bottomRightExit;
		TopRightExit = topRightExit;
		Position = Vector3.Lerp(bottomLeftExit, bottomRightExit, 0.5f);
	}
}

public class Corridor
{
	public List<Vector3> Vertices { get; private set; }
	public Exit From { get; private set; }
	public Exit To { get; private set; }
	public int NumberOfQuads { get; set; }

	public Corridor(Exit from, Exit to, int numberOfQuads)
	{
		From = from;
		To = to;
		NumberOfQuads = numberOfQuads;
	}

	public void SetVertices(List<Vector3> vertices)
	{
		Vertices = vertices;
	}
}