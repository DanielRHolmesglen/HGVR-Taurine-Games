using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    public Rigidbody attachedRigidbody;

    private void Start()
    {
        attachedRigidbody = GetComponent<Rigidbody>();
    }
}
