using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIImageSync : MonoBehaviourPun
{
    public Image uiImage;

    private void Update()
    {
        SetOpacity(uiImage.color.a);
    }

    // 투명도를 설정하는 함수
    public void SetOpacity(float opacity)
    {
        // 로컬 이미지의 알파 값 설정
        UpdateImageOpacity(opacity);

        // RPC 호출로 다른 클라이언트에 동기화
        photonView.RPC("SyncOpacity", RpcTarget.Others, opacity);
    }

    // 로컬에서 알파 값을 변경하는 함수
    private void UpdateImageOpacity(float opacity)
    {
        if (uiImage != null)
        {
            Color color = uiImage.color;
            color.a = Mathf.Clamp01(opacity); // 알파 값은 0~1 사이로 제한
            uiImage.color = color;
        }
    }

    // RPC로 호출되는 함수
    [PunRPC]
    private void SyncOpacity(float opacity)
    {
        UpdateImageOpacity(opacity); // 다른 클라이언트에서 알파 값 적용
    }
}