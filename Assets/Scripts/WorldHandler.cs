using System.Collections.Generic;
using UnityEngine;

public class WorldHandler : MonoBehaviour
{
	private readonly List<BoxCollider> _temporaryColliders = new List<BoxCollider>();

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