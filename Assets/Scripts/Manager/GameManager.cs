using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    public int myCharacter; //0은 타카하시, 1은 야마구치, 2는 이시카와
    public GameObject settingPanelPrefab; // 설정 패널 프리팹
    public GameObject settingPanelInstance; // 생성된 패널 인스턴스
    private string[] characters = { "Player 1", "Player 2", "Player 3" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // 설정 패널 프리팹을 생성
        CreateSettingPanel();
    }

    private void Update()
    {
        // ESC 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingPanel();
        }    
    }

    public void SpawnPlayer(Vector3 spawnPoint)
    {
        PhotonNetwork.Instantiate(characters[myCharacter], spawnPoint, Quaternion.identity);
    }


    public void CreateSettingPanel()
    {
        // 설정 패널 인스턴스가 없으면 프리팹에서 생성
        if (settingPanelPrefab != null && settingPanelInstance == null)
        {
            settingPanelInstance = Instantiate(settingPanelPrefab, FindCanvas());
            settingPanelInstance.SetActive(false); // 처음에는 비활성화
        }
    }

    public void ToggleSettingPanel()
    {
        // 설정 패널의 활성화 상태를 반전
        if (settingPanelInstance != null)
        {
            settingPanelInstance.SetActive(!settingPanelInstance.activeSelf);
        }
    }

    public void OpenSettingPanel()
    {
        if (settingPanelInstance != null)
        {
            settingPanelInstance.SetActive(true);
        }
    }

    public void CloseSettingPanel()
    {
        if (settingPanelInstance != null)
        {
            settingPanelInstance.SetActive(false);
        }
    }

    public Transform FindCanvas()
    {
        // 씬 내에 있는 Canvas를 찾음
        Canvas canvas = FindFirstObjectByType<Canvas>();
        return canvas != null ? canvas.transform : null;
    }
}
