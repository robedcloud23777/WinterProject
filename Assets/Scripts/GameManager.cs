using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject scanObject;
    public bool isAciton;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // �� �ڵ� ����ȭ Ȱ��ȭ
        CreatePlayer(); // �÷��̾ ���� �� �ڵ����� Player ������Ʈ�� ����
    }

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjectData objectData = scanObj.GetComponent<ObjectData>();
    }

    Vector3 GetPlayerSpawnPosition()
    {
        float x = Random.Range(-3f, 3f);  // X��ǥ ���� ����
        float y = Random.Range(-3f, 3f);  // Y��ǥ ���� ����
        return new Vector3(x, y, 0f);  // 2D ���ӿ����� z���� 0���� ����
    }

    private void CreatePlayer()
    {
        // �÷��̾� ���� ��ġ ���
        Vector3 spawnPosition = GetPlayerSpawnPosition();

        // Photon���� ������ �ν��Ͻ�ȭ (Resources ���� �� ��� ���)
        PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);
    }
}
