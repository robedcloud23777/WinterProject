using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    public void Take()
    {
        playerAnimator.SetTrigger("TAKE");
    }

    public void Put()
    {
        playerAnimator.SetTrigger("PUT");
    }

    public void Idle()
    {
        playerAnimator.SetTrigger("IDLE");
    }

    public void Walk()
    {
        playerAnimator.SetTrigger("WALK");
    }

    public void Run()
    {
        playerAnimator.SetTrigger("RUN");
    }

    public void CrouchIdle()
    {
        playerAnimator.SetTrigger("CROUNCH IDLE");
    }

    public void CrouchWalk()
    {
        playerAnimator.SetTrigger("CROUNCH WALK");
    }

    public void CrouchJog()
    {
        playerAnimator.SetTrigger("CROUNCH JOG");
    }

    public void Hit()
    {
        playerAnimator.SetTrigger("HIT1");
    }

    public void Die()
    {
        playerAnimator.SetTrigger("DIE2");// Çìµå¼¦´À³¦
    }

    //ÃÑ ½î´Â ÁßÀÇ ¾Ö´Ï¸ÞÀÌ¼Ç
    public void Shoot()
    {
        playerAnimator.SetTrigger("IDLE 0");
    }

    public void S_Jog()
    {
        playerAnimator.SetTrigger("JOG");
    }
    public void S_WalkForward()
    {
        playerAnimator.SetTrigger("WALK F");
    }
    public void S_WalkBack()
    {
        playerAnimator.SetTrigger("WALK B");
    }
    public void S_WalkRight()
    {
        playerAnimator.SetTrigger("WALK R");
    }

    public void S_WalkLeft()
    {
        playerAnimator.SetTrigger("WALK L");
    }

    public void S_CrounchShoot()
    {
        playerAnimator.SetTrigger("CROUNCH IDLE 0");
    }

    public void S_CrounchWalk()
    {
        playerAnimator.SetTrigger("CROUNCH WALK 0");
    }
}
