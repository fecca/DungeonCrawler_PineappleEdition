using UnityEngine;
using System.Collections.Generic;

public class DungeonObject
{
	public List<Vector3> Vertices { get; private set; }

	public void SetVertices(List<Vector3> vertices)
	{
		Vertices = vertices;
	}
}