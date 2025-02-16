using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private PlayerAnimation playerAnimation;
    private bool isMove;
    private bool isRun;
    private bool isCrouch;
    private bool isZoom;
    private bool isShoot;

    private void Start()
    { 
        if (!photonView.IsMine) return;
        GameManager.Instance.CreateSettingPanel();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        playerBody.transform.localPosition = new Vector3(0, -0.8f, 0); // 애니메이션할 때 튀어나가서 고정
        isRun = playerInput.GetRunInput();
        isCrouch = playerInput.GetCrouchInput();
        isZoom = playerInput.GetZoomInput();
        isShoot = playerInput.GetShootInput();

        if (isRun) playerMovement.moveSpeed = 10.0f;
        else playerMovement.moveSpeed = 5.0f;
        if (isCrouch) playerMovement.moveSpeed /= 3;
        playerMovement.MoveByInput(playerInput.GetMoveInput());

        if (playerInput.GetJumpInput())
        {
            playerMovement.JumpByInput();
        }
        playerMovement.ImplementGravity();

        CurrnetAnimation();
    }

    public void CurrnetAnimation()
    {
        isMove = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        if (isMove)
        {
            if (isRun)
            {
                if (isCrouch)
                {
                    if (isShoot || isZoom) playerAnimation.S_CrounchWalk();
                    else playerAnimation.CrouchJog();
                }
                else
                {
                    if (isShoot || isZoom) playerAnimation.S_Run();
                    else playerAnimation.Run();
                }
            }
            else
            {
                if (isCrouch)
                {
                    if (isShoot || isZoom) playerAnimation.S_CrounchWalk();
                    else playerAnimation.CrouchWalk();
                }
                else
                {
                    if (isShoot || isZoom) playerAnimation.S_Walk();
                    else playerAnimation.Walk();
                }
            }
        }
        else
        {
            if (isCrouch)
            {
                if (isShoot || isZoom) playerAnimation.S_CrounchShoot();
                else playerAnimation.CrouchIdle();
            }
            else
            {
                if (isShoot || isZoom) playerAnimation.Shoot();
                else playerAnimation.Idle();
            }
        }
    }
}
