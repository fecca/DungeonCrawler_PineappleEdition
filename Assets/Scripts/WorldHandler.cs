using System.Collections.Generic;
using UnityEngine;

public class WorldHandler : MonoBehaviour
{
	private List<Collider> _colliders;

	private void Awake()
	{
		_colliders = new List<Collider>();
	}

	public void CreateDungeon()
	{
		var meshFactory = gameObject.AddComponent<MeshFactory>();
		var moduleFactory = new ModuleFactory();

		var dungeonData = moduleFactory.GenerateDungeonData(this);

		DestroyColliders();
		meshFactory.CreateDungeonMeshes(dungeonData);
	}

	public void AddTemporaryCollider(Vector3 position, Vector3 size, Vector3 lookAtPosition)
	{
		var tmpGameObject = new GameObject();
		tmpGameObject.transform.position = position;

		var tmpCollider = tmpGameObject.AddComponent<BoxCollider>();
		tmpCollider.size = size;
		tmpCollider.transform.LookAt(lookAtPosition);

		_colliders.Add(tmpCollider);
	}
	private void DestroyColliders()
	{
		for (var i = _colliders.Count - 1; i >= 0; i--)
		{
			var boxCollider = _colliders[i];
			_colliders.Remove(boxCollider);
			Destroy(boxCollider.gameObject);
		}
	}
}