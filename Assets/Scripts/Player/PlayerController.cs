using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private SkillUi skillUi;
    [SerializeField] private LayerMask obstacleLayer;
    private bool isMove;
    private bool isRun;
    private bool isCrouch;
    private bool isZoom;
    private bool isShoot;
    private bool isSpeedBoost;
    private int[] QAmmo = { 2, 0, 0 };
    private int[] EAmmo = { 1, 2, 1 };

    private void Start()
    {
        skillUi = GameObject.Find("Canvas").GetComponent<SkillUi>();
        if (!photonView.IsMine) return;
        GameManager.Instance.CreateSettingPanel();
        skillUi.InitSkillUi();
        skillUi.InitNickname(PhotonNetwork.LocalPlayer.NickName);
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        playerBody.transform.localPosition = new Vector3(0, -0.8f, 0); // 애니메이션할 때 튀어나가서 고정
        isRun = playerInput.GetRunInput();
        isCrouch = playerInput.GetCrouchInput();
        isZoom = playerInput.GetZoomInput();
        isShoot = playerInput.GetShootInput();

        if (!isSpeedBoost)
        {
            playerMovement.moveSpeed = isRun ? 10.0f : 5.0f;
            if (isCrouch) playerMovement.moveSpeed /= 3;
        }

        if (playerInput.GetQInput()) QSkill(GameManager.Instance.myCharacter);
        if (playerInput.GetEInput()) ESkill(GameManager.Instance.myCharacter);
        PassiveSkill(GameManager.Instance.myCharacter);

        playerMovement.MoveByInput(playerInput.GetMoveInput());
        if (playerInput.GetJumpInput()) playerMovement.JumpByInput();
        playerMovement.ImplementGravity();

        CurrnetAnimation();
    }

    private void CurrnetAnimation()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return;

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

    private void QSkill(int playerNum)
    {
        if (playerNum == 0 && !isSpeedBoost && QAmmo[0] > 0)
        {
            StartCoroutine(SpeedBoostCoroutine(10f, 1.5f));
            skillUi.T_QSkill(QAmmo[0]);
            QAmmo[0]--;
        }
        else if (playerNum == 1)
        {
            return;
        }
        else if (playerNum == 2)
        {
            return;
        }
        else skillUi.CantUse();
    }

    private void ESkill(int playerNum)
    {
        if (playerNum == 0 && EAmmo[0] > 0)
        {
            playerMovement.controller.enabled = false;
            Vector3 targetPosition = transform.position + transform.forward * 10f;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, obstacleLayer))
            {
                targetPosition = hit.point - transform.forward * 0.3f;
            }
            transform.position = targetPosition;
            playerMovement.controller.enabled = true;
            skillUi.T_ESkill(EAmmo[0]);
            EAmmo[0]--;
        }
        else if (playerNum == 1 && EAmmo[1] > 0)
        {
            //데미지 2배
            skillUi.Y_ESkill(EAmmo[1]);
            EAmmo[1]--;
        }
        else if (playerNum == 2 && EAmmo[2] > 0)
        {
            playerMovement.SuperJump();
            skillUi.I_ESkill(EAmmo[2]);
            EAmmo[2]--;
        } else skillUi.CantUse();
    }

    private void PassiveSkill(int playerNum)
    {
        if (playerNum == 2)
        {
            playerMovement.jumpHeight = 2;
        }
    }

    IEnumerator SpeedBoostCoroutine(float duration, float boostMultiplier)
    {
        isSpeedBoost = true;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            playerMovement.moveSpeed = (isRun ? 10.0f : 5.0f);
            if (isCrouch) playerMovement.moveSpeed /= 3;
            playerMovement.moveSpeed *= boostMultiplier;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSpeedBoost = false;
    }
}
