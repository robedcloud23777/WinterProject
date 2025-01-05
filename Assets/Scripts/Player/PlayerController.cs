using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMoveMent;
    [SerializeField] private WeaponHeld weaponHeld;

    private void Update()
    {
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
