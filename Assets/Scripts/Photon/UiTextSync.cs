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
        // 로컬 UI 업데이트
        uiText.text = newText;

        // RPC 호출로 모든 클라이언트에 동기화
        photonView.RPC("SyncText", RpcTarget.All, newText);
    }

    [PunRPC]
    void SyncText(string syncedText)
    {
        uiText.text = syncedText;
    }
}