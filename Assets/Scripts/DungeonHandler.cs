using System.Collections.Generic;
using UnityEngine;

public class DungeonHandler : MonoBehaviour
{
	public List<GameObject> RoomPrefabs = new List<GameObject>();
	public List<GameObject> CorridorPrefabs = new List<GameObject>();
	public int NumberOfModules = 100;

	private List<Module> _modules;
	private int _numberOfConsecutiveCorridors;
	private const int _maximumNumberOfConsecutiveCorridors = 2;

	private void Awake()
	{
		_modules = new List<Module>();
	}

	public void CreateDungeon()
	{
		var modulesToIterate = new List<Module>();
		var bounds = new List<Bounds>();
		var containerGameObject = new GameObject("Dungeon");

		// Create first module
		var gameObject = Instantiate(GetRandomModulePrefabOfType(ModuleType.Room));
		var firstModule = gameObject.GetComponent<Module>();
		modulesToIterate.Add(firstModule);
		bounds.Add(GetModuleBounds(gameObject));

		for (var moduleIndex = 0; moduleIndex < NumberOfModules; moduleIndex++)
		{
			var module = modulesToIterate.GetRandomElement();
			if (module == null)
			{
				break;
			}
			_modules.Add(module);
			module.transform.parent = containerGameObject.transform;

			var exits = module.GetExits();
			for (var exitIndex = 0; exitIndex < exits.Count; exitIndex++)
			{
				// Skip closed exits
				var originalModuleExit = exits[exitIndex];
				if (!originalModuleExit.Open)
				{
					continue;
				}

				// Create new room and find exits
				var newType = GetRandomModuleType(module.Type);
				var newGameObject = Instantiate(GetRandomModulePrefabOfType(newType));
				var newModule = newGameObject.GetComponent<Module>();
				var newModuleExits = newModule.GetExits();
				var exitToMatch = newModuleExits.GetRandomElement();

				// Match exits
				exitToMatch.Transform.gameObject.SnapTo(originalModuleExit.Transform.gameObject);

				// Check if new module intersects with existing modules
				var newModuleBounds = GetModuleBounds(newGameObject);
				if (BoundsIntersect(newModuleBounds, bounds))
				{
					Destroy(newGameObject);
					continue;
				}
				bounds.Add(newModuleBounds);

				// Close the exits of both rooms
				exitToMatch.Open = false;
				originalModuleExit.Open = false;

				modulesToIterate.Add(newModule);
			}
			modulesToIterate.Remove(module);
		}
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
	private bool BoundsIntersect(Bounds objectBounds, List<Bounds> boundsList)
	{
		for (var i = 0; i < boundsList.Count; i++)
		{
			if (objectBounds.Intersects(boundsList[i]))
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
		if (type == ModuleType.Corridor)
		{
			if (_numberOfConsecutiveCorridors >= _maximumNumberOfConsecutiveCorridors)
			{
				_numberOfConsecutiveCorridors = 0;
				return ModuleType.Room;
			}

			if (Random.Range(0, 2) % 2 == 0)
			{
				_numberOfConsecutiveCorridors = 0;
				return ModuleType.Room;
			}
		}

		_numberOfConsecutiveCorridors++;
		return ModuleType.Corridor;
	}
}