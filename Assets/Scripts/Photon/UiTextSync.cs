using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class UITextSync : MonoBehaviourPun
{
    public TMP_Text uiText;

    private void Update()
    {
        UpdateText(uiText.text);
    }

    public void UpdateText(string newText)
    {
        // ���� UI ������Ʈ
        uiText.text = newText;

        // RPC ȣ��� ��� Ŭ���̾�Ʈ�� ����ȭ
        photonView.RPC("SyncText", RpcTarget.All, newText);
    }

    [PunRPC]
    void SyncText(string syncedText)
    {
        uiText.text = syncedText;
    }
}