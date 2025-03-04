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

        // 화면에서 마우스 위치를 기준으로 발사 방향을 구함
        Ray screenRay = mainCamera.ScreenPointToRay(Input.mousePosition); // 마우스 위치
        Vector3 shootDirection = screenRay.direction.normalized; // 반동 없이 그대로 적용

        Ray ray = new Ray(firePoint.position, shootDirection);
        RaycastHit hit;

        // 총구에서 스파크 VFX 생성
        bulletMark.Spark(firePoint);

        // LineRenderer 활성화 및 위치 설정
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position); // 시작점

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

            // LineRenderer 끝 위치 설정 (맞은 지점)
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // 만약 빗나갔다면 끝 위치는 100 유닛 떨어진 지점
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100f);
            launchable.bullet--;
        }

        // 총알 경로를 2초 동안 표시
        Invoke("DisableLineRenderer", 2f);

        Debug.DrawRay(ray.origin, shootDirection * 100f, Color.red, 2f);
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
