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
            // I'm not 100% sure when the primary input device becomes available. This is to save ourselves some
            // suffering.
            primaryInput = VRDevice.Device.PrimaryInputDevice;
            avatarRigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        };
    }

    private void Update()
    {
        // Liminal's SDK seems to recognise my WMR headset as a GearVR device..? As such the controllers aren't
        // functional. This is something to let us spoof controller input only if we want to - it has no runtime cost.
        #if !DEBUG_VR_XDDD
        var movementVector = primaryInput.GetAxis2D(VRAxis.OneRaw).normalized * movementSpeed;
        #else
        var movementVector = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")).normalized * movementSpeed;
        
        #endif

        var forward = Quaternion.Euler(0f, avatarEye.rotation.eulerAngles.y, 0f);
        avatarRigidbody.velocity = forward * new Vector3(movementVector.x, 0f, movementVector.y);
    }

    // I hate doing this but without proper layers I'm not sure if there's much other option. Love you Liminal.
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Hands"))
        {
            Physics.IgnoreCollision(other.collider, collider);
        }
    }
}
