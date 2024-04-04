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
        _zoomAmount = 45.0f;
        _zoomSensitivity = 0.05f;
    }

    private void Update()
    {
        CameraMovement();
        CameraZoom();
    }

    private void LateUpdate()
    {
        
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

    private void CameraZoom()
    {
        if (_scrollY > 0 && _mainCamera.fieldOfView <= 30)
        {
            _mainCamera.fieldOfView = 30;
        }
        else if (_scrollY < 0 && _mainCamera.fieldOfView >= 60)
        {
            _mainCamera.fieldOfView = 60;
        }
        else
        {
            _zoomAmount += -_scrollY * _zoomSensitivity;
            _mainCamera.fieldOfView = Mathf.Clamp(_zoomAmount, 30, 60);
        }
    }

    public void OnZoomInput(InputAction.CallbackContext context)
    {
        _scrollY = context.ReadValue<float>();
        Debug.Log(_scrollY);
    }
}
