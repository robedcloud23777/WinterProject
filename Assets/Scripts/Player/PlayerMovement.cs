using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private CharacterController controller;
    private Vector3 _moveDirection;
    [SerializeField] private float jumpHeight = 2f;
    private const float Gravity = -9.81f;
    [SerializeField] private float gravityMultiply = 1f;
    private Vector3 _velocity;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;

    public void MoveByInput(Vector2 input)
    {
        _moveDirection = transform.forward * input.y + transform.right * input.x;
        _moveDirection *= moveSpeed;

        controller.Move(_moveDirection * Time.deltaTime);
    }

    private bool isGround()
    {
        return Physics.Raycast(groundCheck.position, Vector3.down, groundDistance);
    }

    private float GetGravity()
    {
        return Gravity * gravityMultiply;
    }

    public void JumpByInput()
    {
        if (!isGround()) return;
        _velocity.y = Mathf.Sqrt(jumpHeight * -2f * GetGravity());
    }

    public void ImplementGravity()
    {
        if(isGround() && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += GetGravity() * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }
}
