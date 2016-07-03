using UnityEngine;
using System.Collections.Generic;

public class Old_ModuleHandler : MonoBehaviour
{
	public List<GameObject> RoomPrefabs = new List<GameObject>();
	public List<GameObject> CorridorPrefabs = new List<GameObject>();

	public Old_Module CreateRandomRoomModule()
	{
		var gameObject = Instantiate(GetRandomModulePrefab(ModuleType.Room));

		return gameObject.GetComponent<Old_Module>();
	}
	public Old_Module CreateRandomCorridorModule()
	{
		var gameObject = Instantiate(GetRandomModulePrefab(ModuleType.Corridor));

		return gameObject.GetComponent<Old_Module>();
	}
	public Old_Module CreateModule(ModuleType moduleType)
	{
		var gameObject = Instantiate(GetRandomModulePrefab(moduleType));

		return gameObject.GetComponent<Old_Module>();
	}
	public Old_Module CreateRandomModule(ModuleType previousModuleType)
	{
		var randomModuleType = GetRandomModuleType(previousModuleType);
		var randomModulePrefab = GetRandomModulePrefab(randomModuleType);
		var gameObject = Instantiate(randomModulePrefab);
		var module = gameObject.GetComponent<Old_Module>();

		return module;
	}
	public Bounds GetModuleBounds(Old_Module module)
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