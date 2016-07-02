using System.Collections.Generic;
using UnityEngine;

public class Corridor : MonoBehaviour
{
	private Dictionary<int, List<Vector3>> _exits = new Dictionary<int, List<Vector3>>();

	public void AddExitVertices(Dictionary<int, List<Vector3>> exits)
	{
		_exits = exits;
	}

	public List<Vector3> GetExitVertices()
	{
		return new List<Vector3>(_exits[0]);
	}
}