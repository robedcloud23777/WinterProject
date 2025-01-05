using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMoveMent;
    [SerializeField] private WeaponHeld weaponHeld;


    private void Update()
    {
        if (!photonView.IsMine) return;
        playerMoveMent.MoveByInput(playerInput.GetMoveInput());

        if (playerInput.GetJumpInput())
        {
            playerMoveMent.JumpByInput();
        }
        playerMoveMent.ImplementGravity();

        int num = playerInput.GetNumberInput();
        if(num >= 0)
        {
            weaponHeld.HoldWeapon(num);
        }
    }
}
