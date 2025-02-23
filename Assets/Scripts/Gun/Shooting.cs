using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Camera mainCamera;
    public float delayTime = 0.1f; // 연사 속도 조절
    public float recoilResetTime = 0.5f; // 반동 초기화 시간
    public float recoilRadius = 0.1f; // 반동 원 크기
    public float recoilIncrease = 0.005f; // 반동 증가율
    private bool isDelay;
    private int shotIndex = 0;
    private float lastShotTime;
    public Launchable launchable;
    void Update()
    {
        if (Input.GetMouseButton(0) && !isDelay&&launchable.IsShoot()== true)
        {
            isDelay = true;
            FireRaycast();
            StartCoroutine(CountAttackDelay());
        }

        // 일정 시간 지나면 반동 패턴 초기화
        if (Time.time - lastShotTime > recoilResetTime)
        {
            shotIndex = 0;
            recoilRadius = 0.1f; // 반동 초기화
        }
    }

    void FireRaycast()
    {
        lastShotTime = Time.time;

        // 점점 커지는 원 안의 랜덤한 위치
        Vector2 recoilOffset = GetRecoilOffset();
        Vector3 shootDirection = mainCamera.transform.forward + mainCamera.transform.right * recoilOffset.x + mainCamera.transform.up * recoilOffset.y;
        shootDirection.Normalize();

        Ray ray = new Ray(mainCamera.transform.position, shootDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("물체 맞음");
            launchable.Bullet--;
        }
        else
        {
            Debug.Log("빗나감");
            launchable.Bullet--;
        }

        Debug.DrawRay(ray.origin, shootDirection * 100f, Color.red, 2f);
    }

    Vector2 GetRecoilOffset()
    {
        // 랜덤한 각도 생성
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // 점점 커지는 반동 범위 내에서 랜덤 위치 선택
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
