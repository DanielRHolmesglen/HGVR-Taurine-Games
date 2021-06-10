using System;
using UnityEngine;
using Liminal;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	private Camera _camera;
	private CharacterController _controller;
	private bool _rotationZeroed = true;
	[SerializeField] private float _movementSpeed = 8;

	private void Start()
	{
		_controller = GetComponent<CharacterController>();
		_camera = Camera.main;
	}

	private void Update()
	{
		// This doesn't take deadzones into account but it's.. probably fine.
		var device = VRDevice.Device;
		var leftAxis = device.PrimaryInputDevice.GetAxis2D(VRAxis.OneRaw);
		var rotationAxis = device.SecondaryInputDevice.GetAxis2D(VRAxis.One).x;

		// Poor man's match expression
		var rotationMultiplier =
			rotationAxis > 0.5 ? 1 :
			rotationAxis < -0.5 ? -1 :
			0;
		
		// We only want to rotate if the joystick's position has zeroed since we last rotated. 
		if (_rotationZeroed)
		{
			var newRotation = transform.rotation.eulerAngles;
			newRotation.y += 45 * rotationMultiplier;
			transform.rotation = Quaternion.Euler(newRotation);
		}
		
		// This needs to be updated here so that it actually has an effect.
		_rotationZeroed = rotationMultiplier == 0;

		// Now we apply movement:
		var rawMovement = new Vector3(leftAxis.x, 0, leftAxis.y);
		var correctedMovement = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0) * rawMovement;
		_controller.Move(correctedMovement.normalized * (_movementSpeed * Time.deltaTime));
	}

	private void FixedUpdate()
	{
		if (!_controller.isGrounded)
		{
			_controller.Move(Vector3.down * (9.81f * Time.fixedDeltaTime));
		}
	}
}
