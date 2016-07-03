using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleExit
{
	public Old_Module Module { get; set; }
	public Old_Exit Exit { get; set; }
}

public class Old_Module : MonoBehaviour
{
	public ModuleType Type;
	public GameObject GenerationColliders;
	public GameObject GameColliders;

	private List<Old_Exit> _exits;
	private List<ModuleExit> _leadsTo;
	private ModuleExit _cameFrom;

	public bool IsColliding { get; set; }

	private void Awake()
	{
		_exits = GetComponentsInChildren<Old_Exit>().ToList();
		_leadsTo = new List<ModuleExit>();
	}
	public void OnTriggerEnter(Collider collider)
	{
		IsColliding = true;
	}

	public List<Old_Exit> GetExits()
	{
		return new List<Old_Exit>(_exits);
	}
	public List<Old_Exit> GetOpenExits()
	{
		var tmpList = new List<Old_Exit>();
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
	public void AddLeadsTo(Old_Module module, Old_Exit exit)
	{
		exit.Open = false;
		_leadsTo.Add(new ModuleExit()
		{
			Module = module,
			Exit = exit
		});
	}
	public void AddCameFrom(Old_Module module, Old_Exit exit)
	{
		exit.Open = false;
		_cameFrom = new ModuleExit()
		{
			Module = module,
			Exit = exit
		};
	}
	public void RemoveLeadsTo(Old_Module module)
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
	public void RemoveCameFrom(Old_Module module)
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
	public void EnableGameColliders()
	{
		GenerationColliders.SetActive(false);
		GameColliders.SetActive(true);
	}
}