using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _cameraHolder;

    private float _speed;

    private void Awake()
    {
        _cameraHolder = transform;
        _speed = 50.0f;
    }

    private void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _cameraHolder.position += new Vector3(0f, _speed, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _cameraHolder.position += new Vector3(0f, -_speed, 0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _cameraHolder.position += new Vector3(-_speed, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _cameraHolder.position += new Vector3(_speed, 0f, 0f);
        }
    }
}
