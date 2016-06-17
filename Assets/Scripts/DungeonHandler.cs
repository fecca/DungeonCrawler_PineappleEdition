using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHandler : MonoBehaviour
{
	public int NumberOfModules = 100;

	private ModuleHandler _moduleHandler;
	private GameObject _containerGameObject;
	private List<Module> _modules;
	private List<Bounds> _boundsList;
	private Module _exitRoom;

	public Module StartingRoom
	{
		get
		{
			if (_modules.Count > 0)
			{
				return _modules[0];
			}

			throw new ArgumentException("No rooms to start in.");
		}
	}
	public Module ExitRoom
	{
		get
		{
			if (_exitRoom != null)
			{
				return _exitRoom;
			}

			throw new NotImplementedException("No exit room set.");
		}
	}

	private void Awake()
	{
		_modules = new List<Module>();
		_boundsList = new List<Bounds>();
		_containerGameObject = new GameObject("Dungeon");
	}

	public void CreateDungeon(ModuleHandler moduleHandler)
	{
		_moduleHandler = moduleHandler;

		ClearDungeon();

		var modulesToIterate = new List<Module>();
		var numberOfModulesCreated = 0;

		var firstModule = _moduleHandler.CreateModule(ModuleType.Room);
		_boundsList.Add(_moduleHandler.GetModuleBounds(firstModule));

		modulesToIterate.Insert(0, firstModule);
		firstModule.transform.SetParent(_containerGameObject.transform);
		firstModule.gameObject.name = "Start";
		_modules.Add(firstModule);
		numberOfModulesCreated++;

		while (modulesToIterate.Count > 0)
		{
			var module = modulesToIterate[0];
			Exit exit = null;
			var newModule = TryAddingNewModule(module, out exit);
			if (newModule == null)
			{
				modulesToIterate.RemoveAt(0);
				continue;
			}

			module.AddLinkTo(newModule);
			newModule.AddLinkFrom(module, exit);

			modulesToIterate.Insert(0, newModule);
			newModule.transform.SetParent(_containerGameObject.transform);
			newModule.gameObject.name = "Module" + numberOfModulesCreated;
			_modules.Add(newModule);
			numberOfModulesCreated++;

			if (numberOfModulesCreated > NumberOfModules)
			{
				break;
			}
		}

		TrimCorridors();
		CloseOpenExits();
		OnDungeonCreated();
	}

	private void ClearDungeon()
	{
		var dungeonModules = _containerGameObject.GetComponentsInChildren<Module>();
		for (var i = 0; i < dungeonModules.Length; i++)
		{
			Destroy(dungeonModules[i].gameObject);
		}

		_modules.Clear();
		_boundsList.Clear();

	}
	private Module TryAddingNewModule(Module module, out Exit e)
	{
		e = null;
		var exits = module.GetOpenExits();
		for (var i = 0; i < exits.Count; i++)
		{
			var exit = exits[i];
			if (!exit.Open)
			{
				continue;
			}

			var newModule = _moduleHandler.CreateRandomModule(module.Type);
			var newModuleOpenExits = newModule.GetOpenExits();
			if (newModuleOpenExits.Count <= 0)
			{
				continue;
			}

			var newModuleExit = newModuleOpenExits.GetRandomElement();
			newModuleExit.gameObject.SnapTo(exit.gameObject);

			var newModuleBounds = _moduleHandler.GetModuleBounds(newModule);
			if (BoundsIntersect(newModuleBounds))
			{
				Destroy(newModule.gameObject);
				continue;
			}

			e = exit;
			exit.Open = false;
			newModuleExit.Open = false;
			_boundsList.Add(newModuleBounds);

			return newModule;
		}

		return null;
	}
	private bool BoundsIntersect(Bounds objectBounds)
	{
		for (var i = 0; i < _boundsList.Count; i++)
		{
			if (objectBounds.Intersects(_boundsList[i]))
			{
				return true;
			}
		}

		return false;
	}
	private void TrimCorridors()
	{
		for (var i = _modules.Count - 1; i > 0; i--)
		{
			var module = _modules[i];
			if (module.Type == ModuleType.Corridor)
			{
				if (module.GetOpenExits().Count > 0)
				{
					DestroyModule(module);
				}
			}
		}
	}
	private void CloseOpenExits()
	{

	}
	private void OnDungeonCreated()
	{
		Debug.Log("Dungeon created with " + _modules.Count + " modules.");

		FindExit();
	}
	private void DestroyModule(Module module)
	{
		// Remove links to this module
		var linkFrom = module.GetLinkFrom();
		linkFrom.Module.RemoveLinkTo(module);
		linkFrom.Exit.Open = true;

		// Remove link to this module
		var linksTo = module.GetLinksTo();
		for (var j = 0; j < linksTo.Count; j++)
		{
			var link = linksTo[j];
			link.RemoveLinkFrom(module);
		}

		_modules.Remove(module);
		Destroy(module.gameObject);
	}
	private void FindExit()
	{
		var distanceFromStartingRoom = 0;
		for (var i = 0; i < _modules.Count; i++)
		{
			var module = _modules[i];
			if (module.GetLinksTo().Count <= 0)
			{
				var distance = Vector3.Distance(StartingRoom.transform.position, module.transform.position);
				if (distance > distanceFromStartingRoom)
				{
					_exitRoom = module;
				}
			}
		}
		_exitRoom.gameObject.name = "Exit";
	}
}