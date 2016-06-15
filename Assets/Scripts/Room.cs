using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public bool IsColliding { get; set; }

	public ModuleType Type;
	public List<Transform> ExitPoints = new List<Transform>();

	private List<Exit> Exits;

	private void Awake()
	{
		Exits = new List<Exit>();
		for (var i = 0; i < ExitPoints.Count; i++)
		{
			Exits.Add(new Exit()
			{
				Transform = ExitPoints[i],
				Open = true
			});
		}
	}
	public void OnCollisionEnter(Collision collision)
	{
		IsColliding = true;
	}
	//public void OnDrawGizmos()
	//{
	//	if (DrawBoundingBoxes)
	//	{
	//		var bounds = new Bounds(transform.position, Vector3.one);
	//		foreach (var collider in GetComponentsInChildren<Collider>())
	//		{
	//			bounds.Encapsulate(collider.bounds);
	//		}
	//		bounds.size *= 0.95f;
	//		Gizmos.DrawWireCube(bounds.center, bounds.size);
	//	}
	//}

	public List<Exit> GetExits()
	{
		return new List<Exit>(Exits);
	}
}

public enum ModuleType
{
	None,
	Room,
	Corridor
}