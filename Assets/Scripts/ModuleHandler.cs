using UnityEngine;
using System.Collections.Generic;

public class ModuleHandler : MonoBehaviour
{
	public List<GameObject> RoomPrefabs = new List<GameObject>();
	public List<GameObject> CorridorPrefabs = new List<GameObject>();

	public Module CreateRandomRoomModule()
	{
		var gameObject = Instantiate(GetRandomModulePrefab(ModuleType.Room));

		return gameObject.GetComponent<Module>();
	}
	public Module CreateRandomCorridorModule()
	{
		var gameObject = Instantiate(GetRandomModulePrefab(ModuleType.Corridor));

		return gameObject.GetComponent<Module>();
	}
	public Module CreateModule(ModuleType moduleType)
	{
		var gameObject = Instantiate(GetRandomModulePrefab(moduleType));

		return gameObject.GetComponent<Module>();
	}
	public Module CreateRandomModule(ModuleType previousModuleType)
	{
		var randomModuleType = GetRandomModuleType(previousModuleType);
		var randomModulePrefab = GetRandomModulePrefab(randomModuleType);
		var gameObject = Instantiate(randomModulePrefab);
		var module = gameObject.GetComponent<Module>();

		return module;
	}
	public Bounds GetModuleBounds(Module module)
	{
		var bounds = new Bounds(module.transform.position, Vector3.one);
		var colliders = module.GetComponentsInChildren<Collider>();
		for (var i = 0; i < colliders.Length; i++)
		{
			bounds.Encapsulate(colliders[i].bounds);
		}
		bounds.size *= 0.9f;

		return bounds;
	}

	private ModuleType GetRandomModuleType(ModuleType previousModuleType)
	{
		return previousModuleType == ModuleType.Room ? ModuleType.Corridor : ModuleType.Room;
	}
	private GameObject GetRandomModulePrefab(ModuleType moduleType)
	{
		switch (moduleType)
		{
			case ModuleType.Room:
				return RoomPrefabs.GetRandomElement();
			case ModuleType.Corridor:
				return CorridorPrefabs.GetRandomElement();
			default:
				throw new System.NotSupportedException("Module type not supported");
		}
	}
}