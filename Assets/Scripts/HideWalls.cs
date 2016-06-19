using UnityEngine;
using System.Collections.Generic;

public class HideWalls : MonoBehaviour
{
	public LayerMask LayerMask;
	public Material NormalMaterial;
	public Material TransparentMaterial;

	private Transform _dudeTransform;
	private Transform _mainCamera;
	private List<Transform> _hiddenObjects;

	private void Start()
	{
		_hiddenObjects = new List<Transform>();
		_dudeTransform = transform;
		_mainCamera = GetComponentInChildren<Camera>().transform;
	}

	void Update()
	{
		var direction = _dudeTransform.position - _mainCamera.position;
		var distance = direction.magnitude;
		var hits = Physics.RaycastAll(_mainCamera.position, direction, distance, LayerMask);

		for (var i = 0; i < hits.Length; i++)
		{
			var currentHit = hits[i].transform;
			if (!_hiddenObjects.Contains(currentHit))
			{
				_hiddenObjects.Add(currentHit);
				currentHit.GetComponent<Renderer>().material = TransparentMaterial;
			}
		}

		for (var i = _hiddenObjects.Count - 1; i >= 0; i--)
		{
			var isHit = false;
			for (var j = 0; j < hits.Length; j++)
			{
				if (hits[j].transform == _hiddenObjects[i])
				{
					isHit = true;
					break;
				}
			}

			if (!isHit)
			{
				var wasHidden = _hiddenObjects[i];
				wasHidden.GetComponent<Renderer>().material = NormalMaterial;
				_hiddenObjects.RemoveAt(i);
			}
		}
	}
}