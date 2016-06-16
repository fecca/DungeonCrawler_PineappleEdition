using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonHandler : MonoBehaviour
{
	public List<GameObject> RoomPrefabs = new List<GameObject>();
	public List<GameObject> CorridorPrefabs = new List<GameObject>();
	public int NumberOfModules = 100;

	private List<Module> _modules;
	private List<Bounds> _boundsList;

	private void Awake()
	{
		_modules = new List<Module>();
		_boundsList = new List<Bounds>();
	}

	public void CreateDungeon()
	{
		var modulesToIterate = new List<Module>();
		var containerGameObject = new GameObject("Dungeon");

		// Create first module
		var gameObject = Instantiate(GetRandomModulePrefabOfType(ModuleType.Room));
		var firstModule = gameObject.GetComponent<Module>();
		firstModule.transform.parent = containerGameObject.transform;
		_modules.Add(firstModule);
		_boundsList.Add(GetModuleBounds(gameObject));
		modulesToIterate.Add(firstModule);

		for (var moduleIndex = 0; moduleIndex < NumberOfModules; moduleIndex++)
		{
			var module = modulesToIterate.GetRandomElement();
			if (module == null)
			{
				break;
			}

			var exits = module.GetExits();
			for (var exitIndex = 0; exitIndex < exits.Count; exitIndex++)
			{
				// Skip closed exits
				var moduleExit = exits[exitIndex];
				if (!moduleExit.Open)
				{
					continue;
				}

				var retries = 0;
				while (retries < 1)
				{
					// Create room and find exits
					var newType = GetRandomModuleType(module.Type);
					var newGameObject = Instantiate(GetRandomModulePrefabOfType(newType));
					var newModule = newGameObject.GetComponent<Module>();
					var newModuleExit = newModule.GetExits().GetRandomElement();
					newModuleExit.gameObject.SnapTo(moduleExit.gameObject);
					var newModuleBounds = GetModuleBounds(newGameObject);

					// Check intersection
					if (!BoundsIntersect(newModuleBounds))
					{
						newModule.transform.parent = containerGameObject.transform;
						_modules.Add(newModule);
						_boundsList.Add(newModuleBounds);
						modulesToIterate.Add(newModule);
						newModuleExit.Open = false;
						moduleExit.Open = false;

						break;
					}

					// Destroy intersecting room and go to next loop
					Destroy(newGameObject);
					retries++;
				}
			}
			modulesToIterate.Remove(module);
		}

		OnDungeonCreated();
	}
	public Vector3 GetStartingPosition()
	{
		if (_modules.Count > 0)
		{
			return _modules[0].transform.position;
		}

		return Vector3.zero;
	}

	private Bounds GetModuleBounds(GameObject gameObject)
	{
		var bounds = new Bounds(gameObject.transform.position, Vector3.one);
		var colliders = gameObject.GetComponentsInChildren<Collider>();
		for (var i = 0; i < colliders.Length; i++)
		{
			bounds.Encapsulate(colliders[i].bounds);
		}
		bounds.size *= 0.95f;

		return bounds;
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
	private GameObject GetRandomModulePrefabOfType(ModuleType type)
	{
		switch (type)
		{
			case ModuleType.Room:
				return RoomPrefabs.GetRandomElement();
			case ModuleType.Corridor:
				return CorridorPrefabs.GetRandomElement();
			default:
				throw new System.NotSupportedException("Module type not supported");
		}
	}
	private ModuleType GetRandomModuleType(ModuleType type)
	{
		return type == ModuleType.Room ? ModuleType.Corridor : ModuleType.Room;
		//if (type == ModuleType.Corridor)
		//{
		//	if (_numberOfConsecutiveCorridors >= _maximumNumberOfConsecutiveCorridors)
		//	{
		//		_numberOfConsecutiveCorridors = 0;
		//		return ModuleType.Room;
		//	}

		//	if (Random.Range(0, 2) % 2 == 0)
		//	{
		//		_numberOfConsecutiveCorridors = 0;
		//		return ModuleType.Room;
		//	}
		//}

		//_numberOfConsecutiveCorridors++;
		//return ModuleType.Corridor;
	}
	private void OnDungeonCreated()
	{
		TrimCorridors();
		CloseOpenExits();
	}
	private void TrimCorridors()
	{
		for (var i = _modules.Count - 1; i > 0; i--)
		{
			var module = _modules[i];
			if (ModuleIsOpenCorridor(module))
			{
				_modules.Remove(module);
				Destroy(module.gameObject);
			}
		}
	}
	private bool ModuleIsOpenCorridor(Module module)
	{
		if (module.Type == ModuleType.Corridor)
		{
			var exits = module.GetExits();
			for (var i = 0; i < exits.Count; i++)
			{
				if (exits[i].Open)
				{
					return true;
				}
			}
		}

		return false;
	}
	private void CloseOpenExits()
	{

	}
}