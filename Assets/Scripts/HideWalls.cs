using UnityEngine;
using System.Collections.Generic;

public class HideWalls : MonoBehaviour
{
	public LayerMask LayerMask;

	private Transform _dudeTransform;
	private Transform _mainCamera;
	private List<Transform> _hiddenObjects;

	private void Start()
	{
		_hiddenObjects = new List<Transform>();
		_dudeTransform = GetComponentInParent<CharacterController>().transform;
		_mainCamera = GetComponent<Camera>().transform;
	}

	void Update()
	{
		Vector3 direction = _dudeTransform.position - _mainCamera.position;
		float distance = direction.magnitude;
		RaycastHit[] hits = Physics.RaycastAll(_mainCamera.position, direction, distance, LayerMask);

		for (var i = 0; i < hits.Length; i++)
		{
			Transform currentHit = hits[i].transform;
			if (!_hiddenObjects.Contains(currentHit))
			{
				_hiddenObjects.Add(currentHit);
				currentHit.GetComponent<Renderer>().enabled = false;
			}
		}

		for (int i = _hiddenObjects.Count - 1; i >= 0; i--)
		{
			bool isHit = false;
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
				Transform wasHidden = _hiddenObjects[i];
				wasHidden.GetComponent<Renderer>().enabled = true;
				_hiddenObjects.RemoveAt(i);
			}
		}
	}
}