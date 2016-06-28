﻿using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DudeController : MonoBehaviour
{
	public float Speed = 5.0f;
	public float RotationSpeed = 5.0f;
	public float Gravity = 5.0f;
	public float JumpSpeed = 8.0F;
	public float StrafeSpeed = 0.5F;

	private CharacterController _controller;
	private Vector3 _moveDirection = Vector3.zero;

	private void Awake()
	{
		_controller = GetComponent<CharacterController>();
	}
	private void Update()
	{
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0), Space.World);

		if (_controller.isGrounded)
		{
			var strafeSpeed = Input.GetAxis("Horizontal") * StrafeSpeed;
			_moveDirection = new Vector3(strafeSpeed, 0, Input.GetAxis("Vertical"));
			_moveDirection = transform.TransformDirection(_moveDirection);
			_moveDirection *= Speed;

			if (Input.GetButton("Jump"))
			{
				_moveDirection.y = JumpSpeed;
			}
		}
		_moveDirection.y -= Gravity * Time.deltaTime;
		_controller.Move(_moveDirection * Time.deltaTime);
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + transform.forward);
	}
}