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

    // ������ �����ϴ� �Լ�
    public void SetOpacity(float opacity)
    {
        // ���� �̹����� ���� �� ����
        UpdateImageOpacity(opacity);

        // RPC ȣ��� �ٸ� Ŭ���̾�Ʈ�� ����ȭ
        photonView.RPC("SyncOpacity", RpcTarget.Others, opacity);
    }

    // ���ÿ��� ���� ���� �����ϴ� �Լ�
    private void UpdateImageOpacity(float opacity)
    {
        if (uiImage != null)
        {
            Color color = uiImage.color;
            color.a = Mathf.Clamp01(opacity); // ���� ���� 0~1 ���̷� ����
            uiImage.color = color;
        }
    }

    // RPC�� ȣ��Ǵ� �Լ�
    [PunRPC]
    private void SyncOpacity(float opacity)
    {
        UpdateImageOpacity(opacity); // �ٸ� Ŭ���̾�Ʈ���� ���� �� ����
    }
}