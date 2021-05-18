using System;
using System.Collections;
using System.Collections.Generic;
using Liminal.SDK.Core;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform avatarEye;
    private Rigidbody avatarRigidbody;
    private IVRInputDevice primaryInput;
    private CharacterController controller;

    private void Awake()
    {
        ExperienceApp.Initializing += () => { controller = GetComponent<CharacterController>(); };
    }

    private void Update()
    {
        // I wanted to cache this call but it seems to be updated.
        var movementVector = VRDevice.Device.PrimaryInputDevice.GetAxis2D(VRAxis.OneRaw).normalized * movementSpeed;
        var forward = Quaternion.Euler(0f, avatarEye.rotation.eulerAngles.y, 0f);
        controller.Move(forward * new Vector3(movementVector.x, 0f, movementVector.y) * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var attachedRigidbody = hit.collider.attachedRigidbody;
        if (attachedRigidbody is null || attachedRigidbody.isKinematic || hit.moveDirection.y < -0.3f ||
            hit.gameObject.CompareTag("Player"))
        {
            return;
        }

        var pushDirection = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        attachedRigidbody.velocity = pushDirection * 3.5f;
    }
}