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

    public int myCharacter; //0�� Ÿī�Ͻ�, 1�� �߸���ġ, 2�� �̽�ī��
    public GameObject settingPanelPrefab; // ���� �г� ������
    public GameObject settingPanelInstance; // ������ �г� �ν��Ͻ�
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
        // ���� �г� �������� ����
        CreateSettingPanel();
    }

    private void Update()
    {
        // ESC Ű �Է� ó��
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
        // ���� �г� �ν��Ͻ��� ������ �����տ��� ����
        if (settingPanelPrefab != null && settingPanelInstance == null)
        {
            settingPanelInstance = Instantiate(settingPanelPrefab, FindCanvas());
            settingPanelInstance.SetActive(false); // ó������ ��Ȱ��ȭ
        }
    }

    public void ToggleSettingPanel()
    {
        // ���� �г��� Ȱ��ȭ ���¸� ����
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
        // �� ���� �ִ� Canvas�� ã��
        Canvas canvas = FindFirstObjectByType<Canvas>();
        return canvas != null ? canvas.transform : null;
    }
}
