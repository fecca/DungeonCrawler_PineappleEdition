using System.Collections.Generic;
using UnityEngine;

public class WorldHandler : MonoBehaviour
{
	private readonly List<BoxCollider> _temporaryColliders = new List<BoxCollider>();

	public void OnDrawGizmos()
	{
		for (var i = 0; i < _temporaryColliders.Count; i++)
		{
			//var boxCollider = _temporaryColliders[i].GetComponent<BoxCollider>();
			//Gizmos.DrawWireCube(boxCollider.transform.position, boxCollider.size);
		}
	}

	public void AddTemporaryCollider(BoxCollider boxCollider)
	{
		_temporaryColliders.Add(boxCollider);
	}
	public void DestroyColliders()
	{
		for (var i = _temporaryColliders.Count - 1; i >= 0; i--)
		{
			var boxCollider = _temporaryColliders[i];
			_temporaryColliders.Remove(boxCollider);
			Destroy(boxCollider.gameObject);
		}
	}
}