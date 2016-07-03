using UnityEngine;
using System.Collections.Generic;

public class Module : MonoBehaviour
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

//public class CircularRoom
//{
//	private List<Vector3> _vertices = new List<Vector3>();
//	private List<Vector3> _exitCorners = new List<Vector3>();

//	public Vector3[] GetVertices()
//	{
//		return _vertices.ToArray();
//	}
//}