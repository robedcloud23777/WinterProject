using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMoveMent;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private PlayerAnimation playerAnimation;
    private bool isMove;


    private void Start()
    { 
        if (!photonView.IsMine) return;
        GameManager.Instance.CreateSettingPanel();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        playerBody.transform.localPosition = new Vector3(0, -0.8f, 0); // 애니메이션할 때 튀어나가서 고정
        playerMoveMent.MoveByInput(playerInput.GetMoveInput());

        if (playerInput.GetJumpInput())
        {
            playerMoveMent.JumpByInput();
        }
        playerMoveMent.ImplementGravity();

        CurrnetAnimation();
    }

    public void CurrnetAnimation()
    {
        isMove = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vewrtical") != 0;
        if (isMove)
        {
            playerAnimation.Walk();
        }
    }
}
