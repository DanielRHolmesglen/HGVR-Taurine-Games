using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using UnityEngine;

public class GrabbingHand : MonoBehaviour
{
    public float pickupDistance = 0.3f;
    private Rigidbody _holdingTarget;

    private void Update()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;
        if (!primaryInput.GetButtonDown(VRButton.One)) return;
        
        if (_holdingTarget is null)
        {
            _holdingTarget = Physics
                .OverlapSphere(transform.position, pickupDistance)
                .Select(overlappingCollider => overlappingCollider.GetComponent<Grabbable>()?.attachedRigidbody)
                .First(attachedRigidbody => attachedRigidbody != null);
        }
        else
        {
            _holdingTarget = null;
        }

    }

    private void FixedUpdate()
    {
        if (_holdingTarget is null) return;
        
        var transformRotation = transform.rotation * Quaternion.Inverse(_holdingTarget.transform.rotation);
        // Adjust the velocity of our target to move it towards our hand
        _holdingTarget.velocity = (transform.position - _holdingTarget.transform.position) / Time.fixedDeltaTime;
        _holdingTarget.maxAngularVelocity = 20f;

        var newRotation = new Vector3(
            Mathf.DeltaAngle(0, transformRotation.eulerAngles.x), 
            Mathf.DeltaAngle(0, transformRotation.eulerAngles.y), 
            Mathf.DeltaAngle(0, transformRotation.eulerAngles.z));
        
        newRotation *= Mathf.Deg2Rad;

        // Adjust the angular velocity of the target to rotate it towards our hand
        _holdingTarget.angularVelocity = newRotation / Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}
