using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;

public class PlayerUi : MonoBehaviour
{
    public GameObject[] skillUi;

    public TMP_Text nickname;
    public TMP_Text cantUse;
    public Image[] t_QAmmo;
    public Image[] t_EAmmo;
    public Image[] y_QAmmo;
    public Image[] y_EAmmo;
    public Image[] i_QAmmo;
    public Image[] i_EAmmo;

    public Image[] hpBars;

    public TMP_Text timer;

    public GameObject countdownUi;
    public TMP_Text countdownText;
    public bool isCountdown = false;

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        UpdateTimerDisplay(GameManager.Instance.time);
    }

    public void InitSkillUi()
    {
        skillUi[GameManager.Instance.myCharacter].SetActive(true);
    }

    public void InitNickname(string playerName)
    {
        nickname.text = playerName;
    }

    public void T_QSkill(int ammo)
    {
        SkillUsed(t_QAmmo[ammo - 1]);
    }
    public void T_ESkill(int ammo)
    {
        SkillUsed(t_EAmmo[ammo - 1]);
    }
    public void Y_QSkill(int ammo)
    {
        SkillUsed(y_QAmmo[ammo - 1]);
    }
    public void Y_ESkill(int ammo)
    {
        SkillUsed(y_EAmmo[ammo - 1]);
    }
    public void I_QSkill(int ammo)
    {
        SkillUsed(i_QAmmo[ammo - 1]);
    }
    public void I_ESkill(int ammo)
    {
        SkillUsed(i_EAmmo[ammo - 1]);
    }


    private void SkillUsed(Image ammoImage)
    { 
        Color color = ammoImage.color;
        color.a = 0.3f;
        ammoImage.color = color;
    }

    public void CantUse()
    {
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(cantUse.DOFade(1f, 0.5f).SetEase(Ease.InOutQuad));
        fadeSequence.AppendInterval(1f);
        fadeSequence.Append(cantUse.DOFade(0f, 0.5f).SetEase(Ease.InOutQuad));
        fadeSequence.Play();
    }

    public void UpdateHpDisplay(int hp)
    {
        for (int i = 15; i > hp / 10; i--)
        {
            HpLost(hpBars[i-1]);
        }
    }

    private void HpLost(Image hpBar)
    {
        Color color = hpBar.color;
        color.a = 0.3f;
        hpBar.color = color;
    }

    public void UpdateTimerDisplay(float time)
    {
   
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timer.text = minutes + " : " + seconds;
    }

    public IEnumerator StartCountdown()
    {
        countdownUi.SetActive(true);
        isCountdown = true;
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownUi.SetActive(false);

        isCountdown = false;
        GameManager.Instance.timerIsRunning = true;
    }
}
