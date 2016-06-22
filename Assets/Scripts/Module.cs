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
	private List<ModuleExit> _leadsTo;
	private ModuleExit _cameFrom;

	public bool IsColliding { get; set; }

	private void Awake()
	{
		_exits = GetComponentsInChildren<Exit>().ToList();
		_leadsTo = new List<ModuleExit>();
	}
	public void OnCollisionEnter(Collision collision)
	{
		IsColliding = true;
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
	public void AddLeadsTo(Module module, Exit exit)
	{
		exit.Open = false;
		_leadsTo.Add(new ModuleExit()
		{
			Module = module,
			Exit = exit
		});
	}
	public void AddCameFrom(Module module, Exit exit)
	{
		exit.Open = false;
		_cameFrom = new ModuleExit()
		{
			Module = module,
			Exit = exit
		};
	}
	public void RemoveLeadsTo(Module module)
	{
		for (int i = 0; i < _leadsTo.Count; i++)
		{
			var link = _leadsTo[i];
			if (link.Module == module)
			{
				link.Exit.Open = true;
				_leadsTo.Remove(link);
				break;
			}
		}
	}
	public void RemoveCameFrom(Module module)
	{
		_cameFrom = null;
	}
	public List<ModuleExit> GetLeadsTo()
	{
		return new List<ModuleExit>(_leadsTo);
	}
	public ModuleExit GetCameFrom()
	{
		return _cameFrom;
	}
}