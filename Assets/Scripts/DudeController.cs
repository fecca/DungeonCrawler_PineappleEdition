using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DudeController : MonoBehaviour
{
	public float Speed = 5.0f;
	public float RotationSpeed = 5.0f;
	public float MaxSpeed = 20.0f;

	private Rigidbody _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}
	private void Update()
	{
		if (Input.GetKey(KeyCode.W))
		{
			if (_rigidbody.velocity.z > MaxSpeed)
			{
				return;
			}

			_rigidbody.AddForce(transform.forward * Time.deltaTime * Speed, ForceMode.Impulse);
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(-Vector3.up * Time.deltaTime * RotationSpeed);
		}
		if (Input.GetKey(KeyCode.S))
		{
			if (_rigidbody.velocity.z > MaxSpeed)
			{
				return;
			}

			_rigidbody.AddForce(-transform.forward * Time.deltaTime * Speed);
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(Vector3.up * Time.deltaTime * RotationSpeed);
		}
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + transform.forward);
	}
}