using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Module : MonoBehaviour
{
	public ModuleType Type;

	private List<Exit> Exits;

	private void Awake()
	{
		Exits = GetComponentsInChildren<Exit>().ToList();
	}

	public List<Exit> GetExits()
	{
		return new List<Exit>(Exits);
	}
}