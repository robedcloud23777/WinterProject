using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUi : MonoBehaviour
{
    public GameObject[] skillUi;
    public void InitSkillUi()
    {
        skillUi[GameManager.Instance.myCharacter].SetActive(true);
    }
}
