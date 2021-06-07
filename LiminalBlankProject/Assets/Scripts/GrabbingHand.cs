using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using UnityEngine;

public class GrabbingHand : MonoBehaviour
{
    public float pickupDistance = 0.3f;
    private Rigidbody holdingTarget;

    private void FixedUpdate()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;
        var handClosed = primaryInput.GetButton(VRButton.One) || Input.GetMouseButton(0);

        if (!handClosed)
        {
            holdingTarget = Physics
                .OverlapSphere(transform.position, pickupDistance)
                .Select(collider => collider.GetComponent<Grabbable>()?.attachedRigidbody)
                .First(collider => collider != null);
        }
        
        if (!holdingTarget) return;
        
        var transformRotation = transform.rotation * Quaternion.Inverse(holdingTarget.transform.rotation);
        // Adjust the velocity of our target to move it towards our hand
        holdingTarget.velocity = (transform.position - holdingTarget.transform.position) / Time.fixedDeltaTime;
        holdingTarget.maxAngularVelocity = 20f;

        var newRotation = new Vector3(
            Mathf.DeltaAngle(0, transformRotation.eulerAngles.x), 
            Mathf.DeltaAngle(0, transformRotation.eulerAngles.y), 
            Mathf.DeltaAngle(0, transformRotation.eulerAngles.z));
        
        newRotation *= Mathf.Deg2Rad;

        // Adjust the angular velocity of the target to rotate it towards our hand
        holdingTarget.angularVelocity = newRotation / Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}
