using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public CharacterController controller;
    private Vector3 _moveDirection;
    public float jumpHeight = 2f;
    private const float Gravity = -9.81f;
    [SerializeField] private float gravityMultiply = 1f;
    private Vector3 _velocity;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    private int jumpCount = 0;
    public int maxJumps = 1; // 2단 점프까지 가능하도록 설정

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
        if (isGround())
        {
            jumpCount = 0; // 땅에 닿으면 점프 횟수 초기화
        }

        if (jumpCount < maxJumps)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * GetGravity());
            jumpCount++;
        }
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

    public void SuperJump()
    {
        if (isGround())
        {
            _velocity.y = Mathf.Sqrt(8f * -2f * GetGravity());
            jumpCount = maxJumps;
        }
    }
}
