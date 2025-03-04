using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;
using Photon.Pun.UtilityScripts;

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

    public TMP_Text hp;
    public TMP_Text timer;
    public TMP_Text bullet;
    public TMP_Text kill;
    public TMP_Text death;

    public GameObject countdownUi;
    public TMP_Text countdownText;
    public bool isCountdown = false;

    private float deathTime;

    public Image victoryPanel;
    public Image defeatPanel;
    public Image drawPanel;
    public TMP_Text myName;
    public TMP_Text myKD;
    public TMP_Text otherName;
    public TMP_Text otherKD;

    public Image[] endPanel;
    public TMP_Text[] endPanelText;

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        UpdateTimerDisplay(GameManager.Instance.time);
        if (GameManager.Instance.isDie) DieCountdown();
        if (isCountdown)
        {
            deathTime -= Time.deltaTime;
            int displayTime = (int)deathTime + 1;
            countdownText.text = displayTime.ToString();
            if (deathTime <= 0f)
            {
                countdownUi.SetActive(false);
                isCountdown = false;
                GameManager.Instance.isDie = false;
                GameManager.Instance.isRevival = true;
            }
        }

    }

    public void InitSkillUi(int[] q, int[] e)
    {
        skillUi[GameManager.Instance.myCharacter].SetActive(true);
        if(GameManager.Instance.myCharacter == 0)
        {
            for (int i = 0; i < q[0]; i++) SkillInit(t_QAmmo[i]);
            for (int i = 0; i < e[0]; i++) SkillInit(t_EAmmo[i]);
        }
        else if (GameManager.Instance.myCharacter == 1)
        {
            for (int i = 0; i < q[1]; i++) SkillInit(y_QAmmo[i]);
            for (int i = 0; i < e[1]; i++) SkillInit(y_EAmmo[i]);
        }
        else if (GameManager.Instance.myCharacter == 2)
        {
            for (int i = 0; i < q[2]; i++) SkillInit(i_QAmmo[i]);
            for (int i = 0; i < e[2]; i++) SkillInit(i_EAmmo[i]);
        }
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

    private void SkillInit(Image ammoImage)
    {
        Color color = ammoImage.color;
        color.a = 1f;
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

    public void UpdateHpDisplay(int h)
    {
        if(GameManager.Instance.isDie) return;
        if(h <= 0) return;
        hp.text = h + "";
        for (int i = 15; i > h / 10; i--)
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

    public void HpInit()
    {
        for (int i = 0; i < 15; i++)
        {
            Color color = hpBars[i].color;
            color.a = 1f;
            hpBars[i].color = color;
        }
    }

    public void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timer.text = minutes + " : " + seconds;
    }

    public void UpdateBulletDisplay(int b)
    {
        if (GameManager.Instance.isDie) return;
        bullet.text = b + " / ∞";
    }

    public void UpdateKillDisplay(int k)
    {
        kill.text = "Kill " + k;
    }

    public void UpdateDeathDisplay(int d)
    {
        death.text = "Kill " + d;
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

    private void DieCountdown()
    {
        if (!isCountdown)
        {
            countdownUi.SetActive(true);
            isCountdown = true;
            deathTime = 3f; // 카운트다운을 3초로 리셋
        }
    }

    public void Victory()
    {
        victoryPanel.DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        for(int i = 0; i< endPanel.Length; i++)
        {
            endPanel[i].DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        }
        for(int i = 0;i< endPanelText.Length; i++)
        {
            endPanelText[i].DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        }
    }

    public void Defeat()
    {
        defeatPanel.DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        for (int i = 0; i < endPanel.Length; i++)
        {
            endPanel[i].DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        }
        for (int i = 0; i < endPanelText.Length; i++)
        {
            endPanelText[i].DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        }
    }

    public void Draw()
    {
        drawPanel.DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        for (int i = 0; i < endPanel.Length; i++)
        {
            endPanel[i].DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        }
        for (int i = 0; i < endPanelText.Length; i++)
        {
            endPanelText[i].DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        }
    }

    public void Info(string name1, string kd1, string name2, string kd2)
    {
        myName.text = name1;
        myKD.text = kd1;
        otherName.text = name2;
        otherKD.text = kd2;
        myName.gameObject.SetActive(true);
        myKD.gameObject.SetActive(true);
        otherName.gameObject.SetActive(true);
        otherKD.gameObject.SetActive(true);
    }
}
