using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Header ("Define")]
    private Transform _cameraHolder;
    private Camera _mainCamera;
    private float _speed;

    [Header ("Zoom")]
    [SerializeField] private float _zoomSensitivity;
    private float _scrollY;
    private float _zoomAmount;

    private void Awake()
    {
        _cameraHolder = transform;
        _mainCamera = GetComponentInChildren<Camera>();
        _speed = 50.0f;
        _zoomAmount = 80.0f;
        _zoomSensitivity = 10f;
        _scrollY = 0f;
    }

    private void Update()
    {
        OnZoomInput();
        CameraZoom();
        CameraPosClamp();
    }

    private void FixedUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (Input.GetKey(KeyCode.W) && _cameraHolder.position.y < 3200f)
        {
            _cameraHolder.position += new Vector3(0f, _speed, 0f);
        }
        if (Input.GetKey(KeyCode.S) && _cameraHolder.position.y > -3200f)
        {
            _cameraHolder.position += new Vector3(0f, -_speed, 0f);
        }
        if (Input.GetKey(KeyCode.A) && _cameraHolder.position.x > -3200f)
        {
            _cameraHolder.position += new Vector3(-_speed, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D) && _cameraHolder.position.x < 3200f)
        {
            _cameraHolder.position += new Vector3(_speed, 0f, 0f);
        }
    }

    private void CameraZoom()
    {
        if (_scrollY > 0 && _mainCamera.fieldOfView <= 30)
        {
            _mainCamera.fieldOfView = 30;
        }
        else if (_scrollY < 0 && _mainCamera.fieldOfView >= 120)
        {
            _mainCamera.fieldOfView = 120;
        }
        else
        {
            _zoomAmount += -_scrollY * _zoomSensitivity;
            _mainCamera.fieldOfView = Mathf.Clamp(_zoomAmount, 30, 120);
        }
    }

    private void CameraPosClamp()
    {
        float clampedX = Mathf.Clamp(_cameraHolder.position.x, -3200f, 3200f);
        float clampedY = Mathf.Clamp(_cameraHolder.position.y, -3200f, 3200f);
        _cameraHolder.position = new Vector3(clampedX, clampedY, -1000f);
    }

    private void OnZoomInput()
    {
        _scrollY = Input.GetAxis("Mouse ScrollWheel");
    }
}
