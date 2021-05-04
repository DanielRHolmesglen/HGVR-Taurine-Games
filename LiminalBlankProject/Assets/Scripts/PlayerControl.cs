using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody avatarRigidbody;
    [SerializeField] private float movementSpeed;
    private IVRInputDevice primaryInput;
    
    private void Awake()
    {
        primaryInput = VRDevice.Device.PrimaryInputDevice;
        avatarRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var movementVector = primaryInput.GetAxis2D(VRAxis.One).normalized * movementSpeed;
        avatarRigidbody.velocity = movementVector;
    }
}
