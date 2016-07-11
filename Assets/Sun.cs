using UnityEngine;

public class Sun : MonoBehaviour
{
	[SerializeField]
	private float RotationSpeed = 20;

	private void Update()
	{
		transform.Rotate(Vector3.up, Time.deltaTime * RotationSpeed, Space.World);
	}
}