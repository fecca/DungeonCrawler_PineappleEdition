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
	public List<Exit> Exits
	{
		get
		{
			if (_exits == null)
			{
				throw new System.NotImplementedException("List of exits not set. Call SetVertices first, noob.");
			}

			return _exits;
		}
		private set
		{
			_exits = value;
		}
	}

	private List<Exit> _exits;

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
		_exits = null;
	}

	public void SetVertices(List<Vector3> vertices)
	{
		Vertices = vertices;

		Exits = FindExits();
	}

	private List<Exit> FindExits()
	{
		var groupSize = Vertices.Count / NumberOfCorners;

		var tmpList = new List<Exit>();
		for (var i = 0; i < NumberOfCorners; i++)
		{
			var thisGroup = groupSize * i;
			var nextGroup = i < NumberOfCorners - 1 ? thisGroup + groupSize : 0;
			var thisVertex = Vertices[thisGroup + 6];
			var nextVertex = Vertices[nextGroup + 6];
			var midVertex = Vector3.Lerp(thisVertex, nextVertex, 0.5f);
			var direction = (midVertex - Position).normalized;

			tmpList.Add(new Exit(midVertex, direction));

			//var a = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//a.name = "This" + i;
			//a.transform.position = thisVertex;
			//a.transform.localScale = Vector3.one * 0.2f;

			//var b = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//b.name = "Next" + i;
			//b.transform.position = nextVertex;
			//b.transform.localScale = Vector3.one * 0.2f;

			//var c = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//c.name = "Mid" + i;
			//c.transform.position = midVertex;
			//c.transform.localScale = Vector3.one * 0.3f;
		}

		return tmpList;
	}
}

public class Exit
{
	public Vector3 Position { get; private set; }
	public Vector3 Direction { get; private set; }

	public Exit(Vector3 position, Vector3 direction)
	{
		Position = position;
		Direction = direction;
	}
}