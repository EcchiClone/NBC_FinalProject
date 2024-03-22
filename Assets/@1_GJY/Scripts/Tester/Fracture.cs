using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    public void Explode()
    {
        foreach(Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 rand = Random.insideUnitSphere;
            Debug.Log(rand);
            rb.AddForce(Random.insideUnitSphere * 50f, ForceMode.Impulse);
        }            
    }
}
