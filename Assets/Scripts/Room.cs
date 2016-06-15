using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public bool IsColliding { get; set; }

	public List<Transform> ExitPoints = new List<Transform>();
	public ModuleType Type;

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
		//IsColliding = true;
		//Debug.Log("Collided");
	}

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