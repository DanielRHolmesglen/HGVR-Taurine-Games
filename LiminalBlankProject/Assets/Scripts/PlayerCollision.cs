using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var attachedRigidbody = hit.collider.attachedRigidbody;
        if (attachedRigidbody == null || attachedRigidbody.isKinematic || hit.moveDirection.y < -0.3)
        {
            return;
        }
        
        var direction = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        attachedRigidbody.velocity = direction * 3.5f;
    }
}
