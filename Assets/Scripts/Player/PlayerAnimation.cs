using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    public void Take()
    {
        playerAnimator.SetTrigger("TAKE");
        StartCoroutine(ResetTriggerAfterFrame("TAKE"));
    }

    public void Put()
    {
        playerAnimator.SetTrigger("PUT");
        StartCoroutine(ResetTriggerAfterFrame("PUT"));
    }

    public void Idle()
    {
        playerAnimator.SetTrigger("IDLE");
        StartCoroutine(ResetTriggerAfterFrame("IDLE"));
    }

    public void Walk()
    {
        playerAnimator.SetTrigger("WALK");
        StartCoroutine(ResetTriggerAfterFrame("WALK"));
    }

    public void Run()
    {
        playerAnimator.SetTrigger("RUN");
        StartCoroutine(ResetTriggerAfterFrame("RUN"));
    }

    public void CrouchIdle()
    {
        playerAnimator.SetTrigger("CROUCH IDLE");
        StartCoroutine(ResetTriggerAfterFrame("CROUCH IDLE"));
    }

    public void CrouchWalk()
    {
        playerAnimator.SetTrigger("CROUCH WALK");
        StartCoroutine(ResetTriggerAfterFrame("CROUCH WALK"));
    }

    public void CrouchJog()
    {
        playerAnimator.SetTrigger("CROUCH JOG");
        StartCoroutine(ResetTriggerAfterFrame("CROUCH JOG"));
    }

    public void Hit()
    {
        playerAnimator.SetTrigger("HIT1");
        StartCoroutine(ResetTriggerAfterFrame("HIT1"));
    }

    public void Die()
    {
        playerAnimator.SetTrigger("DIE2");
        StartCoroutine(ResetTriggerAfterFrame("DIE2"));
    }

    public void Shoot()
    {
        playerAnimator.SetTrigger("IDLE 0");
        StartCoroutine(ResetTriggerAfterFrame("IDLE 0"));
    }

    public void S_Run()
    {
        playerAnimator.SetTrigger("JOG");
        StartCoroutine(ResetTriggerAfterFrame("JOG"));
    }

    public void S_Walk()
    {
        playerAnimator.SetTrigger("WALK F");
        StartCoroutine(ResetTriggerAfterFrame("WALK F"));
    }

    public void S_CrounchShoot()
    {
        playerAnimator.SetTrigger("CROUCH IDLE 0");
        StartCoroutine(ResetTriggerAfterFrame("CROUCH IDLE 0"));
    }

    public void S_CrounchWalk()
    {
        playerAnimator.SetTrigger("CROUCH WALK 0");
        StartCoroutine(ResetTriggerAfterFrame("CROUCH WALK 0"));
    }

    IEnumerator ResetTriggerAfterFrame(string triggerName)
    {
        yield return null; // 한 프레임 대기
        playerAnimator.ResetTrigger(triggerName);
    }
}
