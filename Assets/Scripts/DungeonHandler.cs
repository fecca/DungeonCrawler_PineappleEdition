using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHandler : MonoBehaviour
{
	public bool CullModules;
	public int CullingDistance = 50;
	public int NumberOfModules = 100;

	private ModuleHandler _moduleHandler;
	private List<Module> _modules;
	private GameObject _containerGameObject;
	private Module _exitRoom;
	private float _cullingTimer;
	private int _numberOfModulesCreated = 0;

	private const float CullingTime = 1.0f;

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
		_containerGameObject = new GameObject("Dungeon");
	}
	private void Update()
	{
		if (CullModules)
		{
			if (_cullingTimer > CullingTime)
			{
				_cullingTimer = 0;

				if (Game.PlayerTransform != null)
				{
					for (var i = 0; i < _modules.Count; i++)
					{
						var withinDistance = Vector3.Distance(_modules[i].transform.position, Game.PlayerTransform.position) < CullingDistance;
						_modules[i].gameObject.SetActive(withinDistance);
					}
				}
			}
			_cullingTimer += Time.deltaTime;
		}
	}

	public void CreateDungeon(ModuleHandler moduleHandler)
	{
		_moduleHandler = moduleHandler;

		ClearDungeon();

		var modulesToIterate = new List<Module>();

		var firstModule = _moduleHandler.CreateModule(ModuleType.Room);

		modulesToIterate.Insert(0, firstModule);
		firstModule.transform.SetParent(_containerGameObject.transform);
		firstModule.gameObject.name = "Start";
		_modules.Add(firstModule);
		_numberOfModulesCreated++;

		StartCoroutine(CreateModules(modulesToIterate));
	}

	private IEnumerator CreateModules(List<Module> modulesToIterate)
	{
		while (modulesToIterate.Count > 0)
		{
			var module = modulesToIterate[0];

			Module newModule = null;
			var exits = module.GetOpenExits();
			for (var i = 0; i < exits.Count; i++)
			{
				var exit = exits[i];
				if (!exit.Open)
				{
					continue;
				}

				var tmpModule = _moduleHandler.CreateRandomModule(module.Type);
				var newModuleOpenExits = tmpModule.GetOpenExits();
				if (newModuleOpenExits.Count <= 0)
				{
					continue;
				}

				var newModuleExit = newModuleOpenExits.GetRandomElement();
				newModuleExit.gameObject.SnapTo(exit.gameObject);

				yield return new WaitForFixedUpdate();

				if (tmpModule.IsColliding)
				{
					Destroy(tmpModule.gameObject);
					continue;
				}

				module.AddLeadsTo(tmpModule, newModuleExit);
				tmpModule.AddCameFrom(module, exit);

				newModule = tmpModule;
				break;
			}

			if (newModule == null)
			{
				modulesToIterate.RemoveAt(0);
				continue;
			}

			modulesToIterate.Insert(0, newModule);
			newModule.transform.SetParent(_containerGameObject.transform);
			newModule.gameObject.name = "Module" + _numberOfModulesCreated;
			_modules.Add(newModule);
			_numberOfModulesCreated++;

			if (_numberOfModulesCreated > NumberOfModules)
			{
				break;
			}
		}

		DisableConvexColliders();
		TrimCorridors();
		FindExit();

		MessageHub.Instance.Publish(new DungeonCreatedMessage(null));
	}
	private void DisableConvexColliders()
	{
		for (var i = 0; i < _modules.Count; i++)
		{
			var module = _modules[i];

			var rigidbody = module.GetComponent<Rigidbody>();
			rigidbody.isKinematic = true;

			var colliders = module.GetComponentsInChildren<MeshCollider>();
			for (var j = 0; j < colliders.Length; j++)
			{
				colliders[j].convex = false;
			}
		}
	}
	private void ClearDungeon()
	{
		var dungeonModules = _containerGameObject.GetComponentsInChildren<Module>();
		for (var i = 0; i < dungeonModules.Length; i++)
		{
			Destroy(dungeonModules[i].gameObject);
		}

		_modules.Clear();
	}
	private void TrimCorridors()
	{
		for (var i = _modules.Count - 1; i >= 0; i--)
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
	private void DestroyModule(Module module)
	{
		// Remove links to this module
		var cameFrom = module.GetCameFrom();
		cameFrom.Module.RemoveLeadsTo(module);
		cameFrom.Exit.Open = true;

		// Remove link to this module
		var leadsTo = module.GetLeadsTo();
		for (var j = 0; j < leadsTo.Count; j++)
		{
			var link = leadsTo[j];
			link.Module.RemoveCameFrom(module);
		}

		_modules.Remove(module);
		Destroy(module.gameObject);
	}
	private void FindExit()
	{
		var distanceFromStartingRoom = 0;
		_exitRoom = StartingRoom;
		for (var i = 0; i < _modules.Count; i++)
		{
			var module = _modules[i];
			if (module.GetLeadsTo().Count <= 0)
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