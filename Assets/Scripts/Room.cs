using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
	private List<Vector3> _exits = new List<Vector3>();

	public void AddExitVertices(List<Vector3> exits)
	{
		_exits = exits;
	}

	public List<Vector3> GetExitVertices()
	{
		return new List<Vector3>(_exits);
	}
}