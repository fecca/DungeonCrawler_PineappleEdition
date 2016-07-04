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
	public List<List<Vector3>> ExitVertices { get; set; }

	public Room(Vector3 position, int numberOfCorners, int radius, int height, int thickness, int numberOfExits)
	{
		Position = position;
		NumberOfCorners = numberOfCorners;
		Radius = radius;
		Height = height;
		Thickness = thickness;
		NumberOfExits = numberOfExits;
		ExitVertices = new List<List<Vector3>>();
	}
}