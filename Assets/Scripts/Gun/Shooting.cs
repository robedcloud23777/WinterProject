using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviourPun
{
    public Camera mainCamera;
    public Transform firePoint; // 총구 위치
    public float delayTime = 0.1f; // 연사 속도 조절
    private bool isDelay;
    private float lastShotTime;
    public PlayerInput playerInput;
    public Launchable launchable;
    public BulletMark bulletMark;
    public int damage = 10;

    public LineRenderer lineRenderer;

    void Update()
    {
        if (!photonView.IsMine) return;
        if (!GameManager.Instance.isDie && playerInput.GetShootInput() && !isDelay && launchable.IsShoot())
        {
            isDelay = true;
            FireRaycast();
            StartCoroutine(CountAttackDelay());
        }

        if (playerInput.GetRInput() && launchable.bullet != 25)
            StartCoroutine(launchable.Reload());
    }

    private void FireRaycast()
    {
        lastShotTime = Time.time;

        // 카메라의 중앙에서 마우스 위치를 기준으로 발사 방향을 구함
        Ray screenRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;

        if (Physics.Raycast(screenRay, out RaycastHit hitInfo))
        {
            targetPoint = hitInfo.point; // 목표 지점 (충돌한 위치)
        }
        else
        {
            targetPoint = screenRay.origin + screenRay.direction * 100f; // 충돌이 없으면 먼 거리 설정
        }

        // firePoint에서 targetPoint까지의 방향을 다시 계산
        Vector3 shootDirection = (targetPoint - firePoint.position).normalized;

        // Ray 설정
        Ray ray = new Ray(firePoint.position, shootDirection);
        RaycastHit hit;

        // 총구에서 스파크 VFX 생성
        bulletMark.Spark(firePoint);

        Vector3 hitPosition = firePoint.position + shootDirection * 100f;

        if (Physics.Raycast(ray, out hit))
        {
            string hitLayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            launchable.bullet--;

            if (hitLayerName == "Player")
            {
                PhotonView targetPhotonView = hit.collider.GetComponent<PhotonView>();
                if (!targetPhotonView.IsMine)
                {
                    targetPhotonView.RPC("GetDamage", RpcTarget.All, damage, photonView.ViewID);
                }
            }
            else if (hitLayerName == "Map")
            {
                // 총알 자국 효과 생성
                bulletMark.MakeMark(hit);
            }
        }
        else
        {
            launchable.bullet--;
        }

        // 총알 경로를 1초 동안 표시
        Invoke("DisableLineRenderer", 1f);

        Debug.DrawRay(ray.origin, shootDirection * 100f, Color.red, 2f);
        photonView.RPC("SyncShot", RpcTarget.All, firePoint.position, hitPosition);
    }

    [PunRPC]
    private void SyncShot(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        Invoke("DisableLineRenderer", 1f);
    }



    private void DisableLineRenderer()
    {
        lineRenderer.enabled = false;
    }

    IEnumerator CountAttackDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
}
