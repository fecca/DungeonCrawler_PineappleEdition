using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
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

	public List<Exit> GetExits()
	{
		return new List<Exit>(Exits);
	}
}