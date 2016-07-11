using UnityEngine;
using System.Collections.Generic;

public class DungeonObject
{
	public ModuleType Type { get; protected set; }
	public List<Vector3> Vertices { get; private set; }

	public virtual void SetVertices(List<Vector3> vertices)
	{
		Vertices = vertices;
	}
}