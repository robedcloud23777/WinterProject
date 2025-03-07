using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviourPun
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerHead;
    [SerializeField] private Transform cam;
    [SerializeField] private float cameraDistance = 2.5f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private LayerMask collisionLayer;
    private Vector2 _mouseInput;
    private float _currentRotationY = 0f;
    [SerializeField] private float minVerticalAngle = -45f;
    [SerializeField] private float maxVerticalAngle = 45f;
    private bool isTPS = false;
    private bool isMove;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!photonView.IsMine)
        {
            // 다른 플레이어의 카메라 비활성화
            cameraObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        isMove = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        if (isTPS) playerHead.localPosition = new Vector3(0.3f, 1.0f, 0);
        else playerHead.localPosition = new Vector3(0f, 0.6f, 0);

        if (playerController.isRun && isMove) playerHead.localPosition += new Vector3(0, 0, 0.4f);
        if (playerController.isCrouch)
        {
            if (isMove) playerHead.localPosition += new Vector3(0, -0.3f, 0.35f);
            else playerHead.localPosition += new Vector3(0, -0.3f, 0.1f);
        }
        
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine) return;
        if (GameManager.Instance.settingPanelInstance.activeSelf) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (Cursor.lockState == CursorLockMode.None) return;
            isTPS = !isTPS;
            cam.position = playerHead.position;
        }
        if (isTPS) TPSmode();
        else FPSmode();
    }

    private void FPSmode()
    {
        if (Cursor.lockState == CursorLockMode.None) return;
        _mouseInput.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        _mouseInput.y = Input.GetAxis("Mouse Y") * mouseSensitivity;
        playerBody.Rotate(Vector3.up, _mouseInput.x);
        _currentRotationY -= _mouseInput.y;
        _currentRotationY = Mathf.Clamp(_currentRotationY, minVerticalAngle, maxVerticalAngle);
        cam.localRotation = Quaternion.Euler(_currentRotationY, 0, 0);
    }

    private void TPSmode()
    {
        if (Cursor.lockState == CursorLockMode.None) return;
        _mouseInput.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        _mouseInput.y = Input.GetAxis("Mouse Y") * mouseSensitivity;
        playerBody.Rotate(Vector3.up, _mouseInput.x);
        _currentRotationY -= _mouseInput.y;
        _currentRotationY = Mathf.Clamp(_currentRotationY, minVerticalAngle, maxVerticalAngle);
        cam.localRotation = Quaternion.Euler(_currentRotationY, 0, 0);

        Vector3 desiredPosition = playerHead.position + cam.forward * -cameraDistance + cam.right * 0.3f;
        Vector3 finalPosition = CheckCameraCollision(desiredPosition);

        cam.position = finalPosition;
    }

    private Vector3 CheckCameraCollision(Vector3 desiredPosition)
    {
        if (Physics.Linecast(playerHead.position, desiredPosition, out RaycastHit hit, collisionLayer))
        {
            return hit.point + hit.normal * 0.1f;
        }
        else
        {
            return desiredPosition;
        }
    }
}