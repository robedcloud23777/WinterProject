using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Camera mainCamera;
    public Transform firePoint; // 총구 위치
    public float delayTime = 0.1f; // 연사 속도 조절
    public float recoilResetTime = 0.5f; // 반동 초기화 시간
    public float recoilRadius = 0.1f; // 반동 원 크기
    public float recoilIncrease = 0.005f; // 반동 증가율
    private bool isDelay;
    private int shootIndex = 0;
    private float lastShotTime;
    public PlayerInput playerInput;
    public Launchable launchable;
    public BulletMark bulletMark;

    void Update()
    {
        if (playerInput.GetShootInput() && !isDelay && launchable.IsShoot() == true)
        {
            isDelay = true;
            FireRaycast();
            StartCoroutine(CountAttackDelay());
        }

        // 일정 시간 지나면 반동 패턴 초기화
        if (Time.time - lastShotTime > recoilResetTime)
        {
            shootIndex = 0;
            recoilRadius = 0.1f; // 반동 초기화
        }

        if (playerInput.GetRInput() && launchable.bullet != 25)
            launchable.Reload();
    }

    private void FireRaycast()
    {
        lastShotTime = Time.time;

        // 화면 중앙에서 발사 방향을 구함
        Ray screenRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        // 총구 위치에서 화면 중앙 방향으로 Ray 발사
        Vector2 recoilOffset = GetRecoilOffset();
        Vector3 shootDirection = screenRay.direction + mainCamera.transform.right * recoilOffset.x + mainCamera.transform.up * recoilOffset.y;
        shootDirection.Normalize();

        Ray ray = new Ray(firePoint.position, shootDirection);
        RaycastHit hit;

        // 총구에서 스파크 VFX 생성
        //Instantiate(bulletMark.sparkVFX, firePoint.position, firePoint.rotation);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("맞은 물체의 레이어: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            string hitLayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            launchable.bullet--;

            if (hitLayerName == "Player")
            {
                // 데미지 로직 추가 가능
                launchable.bullet--;
            }
            else if (hitLayerName == "map")
            {
                // 총알 자국 효과 생성
                //bulletMark.MakeMark(hit.point, hit.normal);
                launchable.bullet--;
            }
        }
        else
        {
            Debug.Log("빗나감");
            launchable.bullet--;
        }

        Debug.DrawRay(ray.origin, shootDirection * 100f, Color.red, 2f);
    }

    Vector2 GetRecoilOffset()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomRadius = Random.Range(0f, recoilRadius);
        float x = Mathf.Cos(randomAngle) * randomRadius;
        float y = Mathf.Sin(randomAngle) * randomRadius;

        recoilRadius += recoilIncrease; // 반동 점점 증가

        return new Vector2(x, y);
    }

    IEnumerator CountAttackDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
}
