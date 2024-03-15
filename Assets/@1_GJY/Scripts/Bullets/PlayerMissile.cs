using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    public float power;

    private Rigidbody _rigid;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        _rigid.velocity = transform.forward * power;
    }
}
