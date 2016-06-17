using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleExit
{
	public Module Module { get; set; }
	public Exit Exit { get; set; }
}

public class Module : MonoBehaviour
{
	public ModuleType Type;

	private List<Exit> _exits;
	private List<Module> _linksTo;
	private ModuleExit _linkFrom;

	private void Awake()
	{
		_exits = GetComponentsInChildren<Exit>().ToList();
		_linksTo = new List<Module>();
	}

	public List<Exit> GetExits()
	{
		return new List<Exit>(_exits);
	}
	public List<Exit> GetOpenExits()
	{
		var tmpList = new List<Exit>();
		for (int i = 0; i < _exits.Count; i++)
		{
			var exit = _exits[i];
			if (exit.Open)
			{
				tmpList.Add(exit);
			}
		}

		return tmpList;
	}
	public void AddLinkTo(Module module)
	{
		_linksTo.Add(module);
	}
	public void AddLinkFrom(Module module, Exit exit)
	{
		_linkFrom = new ModuleExit()
		{
			Module = module,
			Exit = exit
		};
	}
	public void RemoveLinkTo(Module module)
	{
		_linksTo.Remove(module);
	}
	public void RemoveLinkFrom(Module module)
	{
		_linksTo.Remove(module);
	}
	public List<Module> GetLinksTo()
	{
		return new List<Module>(_linksTo);
	}
	public ModuleExit GetLinkFrom()
	{
		return _linkFrom;
	}
}