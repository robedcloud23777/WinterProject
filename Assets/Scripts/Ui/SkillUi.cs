using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;

public class SkillUi : MonoBehaviour
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
}
