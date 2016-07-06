using System.Collections.Generic;
using UnityEngine;

public class WorldHandler : MonoBehaviour
{
	private List<GameObject> _temporaryColliders = new List<GameObject>();

	public void AddTemporaryCollider(GameObject collider)
	{
		_temporaryColliders.Add(collider);
	}
	public void DestroyColliders()
	{
		for (var i = _temporaryColliders.Count - 1; i >= 0; i--)
		{
			var collider = _temporaryColliders[i];
			_temporaryColliders.Remove(collider);
			Destroy(collider);
		}
	}
}