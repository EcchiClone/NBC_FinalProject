using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rigidbodyTest : MonoBehaviour
{
    private Rigidbody _rigidBody = null;
    private BoxCollider _boxCollider = null;

    [Header("Config")]
    [SerializeField][Range(0.0f, 10.0f)] float _desireColliderFloatHeight = 2.0f;
    [SerializeField][Range(0.0f, 100.0f)] float _floatDistanceModifier = 10.0f;
    [SerializeField][Range(0.0f, 100.0f)] float _floatVelModifier = 2.0f;

    [Header("Collider Float Values")]
    [SerializeField] Vector3 _calculatedForce = Vector3.zero;
    [SerializeField] float _centerToGroundDistance = 0.0f;
    [SerializeField] float _floatForce = 0.0f;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    /*// Update is called once per frame
    void FixedUpdate()
    {
        float 
    }

    private float Float()
    {

    }*/
}
