#define DEBUG_VR_XDDDA

using System;
using System.Collections;
using System.Collections.Generic;
using Liminal.SDK.Core;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform avatarEye;
    private Rigidbody avatarRigidbody;
    private IVRInputDevice primaryInput;
    private Collider collider;
    
    private void Awake()
    {
        ExperienceApp.Initializing += () =>
        {
            avatarRigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        };
    }

    private void Update()
    {
        // Liminal's SDK seems to recognise my WMR headset as a GearVR device..? As such the controllers aren't
        // functional. This is something to let us spoof controller input only if we want to - it has no runtime cost.
        #if !DEBUG_VR_XDDD
        // I wanted to cache this call but it seems to be updated.
        var movementVector = VRDevice.Device.PrimaryInputDevice.GetAxis2D(VRAxis.OneRaw).normalized * movementSpeed;
        #else
        var movementVector = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")).normalized * movementSpeed;
        
        #endif

        var forward = Quaternion.Euler(0f, avatarEye.rotation.eulerAngles.y, 0f);
        avatarRigidbody.velocity = forward * new Vector3(movementVector.x, 0f, movementVector.y);
    }
}
