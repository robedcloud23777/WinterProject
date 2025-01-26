using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMoveMent;
    [SerializeField] private WeaponHeld weaponHeld;


    private void Start()
    { 
        if (!photonView.IsMine) return;
        GameManager.Instance.CreateSettingPanel();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (GameManager.Instance.settingPanelInstance.activeSelf) playerInput.enabled = true;
        else playerInput.enabled = false;
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
