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
    
    private void Awake()
    {
        ExperienceApp.Initializing += () =>
        {
            primaryInput = VRDevice.Device.PrimaryInputDevice;
            avatarRigidbody = GetComponent<Rigidbody>();
        };
    }

    private void Update()
    {
        #if !DEBUG_VR
        var movementVector = primaryInput.GetAxis2D(VRAxis.OneRaw).normalized * movementSpeed;
        #else
        var movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * movementSpeed;
        #endif

        var forward = Quaternion.Euler(0f, avatarEye.rotation.eulerAngles.y, 0f);
        avatarRigidbody.velocity = forward * new Vector3(movementVector.x, 0f, movementVector.y);
    }
}
