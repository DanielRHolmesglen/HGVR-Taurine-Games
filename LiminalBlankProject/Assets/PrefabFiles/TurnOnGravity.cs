using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnGravity : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Untagged")
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("Whats touching this fucking thing");
        }
    }





}