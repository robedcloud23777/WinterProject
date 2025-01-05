using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerHead;
    [SerializeField] private Transform cam;
    [SerializeField] private float mouseSensitivity = 2f;
    private Vector2 _mouseInput;
    private float _currentRotationY = 0f;
    [SerializeField] private float minVerticalAngle = -45f;
    [SerializeField] private float maxVerticalAngle = 45f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        _mouseInput.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        _mouseInput.y = Input.GetAxis("Mouse Y") * mouseSensitivity;
        playerBody.Rotate(Vector3.up, _mouseInput.x);
        _currentRotationY -= _mouseInput.y;
        _currentRotationY = Mathf.Clamp(_currentRotationY,minVerticalAngle,maxVerticalAngle);
        cam.localRotation = Quaternion.Euler(_currentRotationY, 0, 0);

    }
}