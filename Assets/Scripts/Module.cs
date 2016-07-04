using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Module : MonoBehaviour
{
	//public RoomModel Model { get; set; }

	//private List<Vector3> _exits = new List<Vector3>();

	//public void AddExitVertices(List<Vector3> exits)
	//{
	//	_exits = exits;
	//}

	//public List<Vector3> GetExitVertices()
	//{
	//	return new List<Vector3>(_exits);
	//}
}

public class Room : Module
{
}

public class Corridor : Module
{
}

public class RoomModel
{
	public Vector3 Position { get; private set; }
	public int NumberOfCorners { get; private set; }
	public int Radius { get; private set; }
	public int Height { get; private set; }
	public int Thickness { get; private set; }
	//public List<Vector3> UniqueVertices { get; set; }
	//public List<Vector3> SharedVertices { get; set; }
	//public List<int> Triangles { get; set; }
	public List<Vector3> ExitVertices { get; set; }
	//public int ExitCorner { get; set; }

	public RoomModel(Vector3 position, int numberOfCorners, int radius, int height, int thickness)
	{
		Position = position;
		NumberOfCorners = numberOfCorners;
		Radius = radius;
		Height = height;
		Thickness = thickness;
		ExitVertices = new List<Vector3>();
	}
}