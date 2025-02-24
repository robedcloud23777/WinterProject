using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Camera mainCamera;
    public float delayTime = 0.1f; // ���� �ӵ� ����
    public float recoilResetTime = 0.5f; // �ݵ� �ʱ�ȭ �ð�
    public float recoilRadius = 0.1f; // �ݵ� �� ũ��
    public float recoilIncrease = 0.005f; // �ݵ� ������
    private bool isDelay;
    private int shotIndex = 0;
    private float lastShotTime;
    public Launchable launchable;
    public BulletMark bulletMark;



    void Update()
    {
        if (Input.GetMouseButton(0) && !isDelay && launchable.IsShoot() == true)
        {
            isDelay = true;
            FireRaycast();
            StartCoroutine(CountAttackDelay());
        }

        // ���� �ð� ������ �ݵ� ���� �ʱ�ȭ
        if (Time.time - lastShotTime > recoilResetTime)
        {
            shotIndex = 0;
            recoilRadius = 0.1f; // �ݵ� �ʱ�ȭ
        }
    }

    void FireRaycast()
    {
        lastShotTime = Time.time;

        Vector2 recoilOffset = GetRecoilOffset();
        Vector3 shootDirection = mainCamera.transform.forward + mainCamera.transform.right * recoilOffset.x + mainCamera.transform.up * recoilOffset.y;
        shootDirection.Normalize();

        Ray ray = new Ray(mainCamera.transform.position, shootDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("���� ��ü�� ���̾�: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            string hitLayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            launchable.Bullet--;

            if (hitLayerName == "Player")
            {
                //���ؾ� �ϰ���
                launchable.Bullet--;
            }
            else if (hitLayerName == "map")
            {
                bulletMark.MakeMark();
                launchable.Bullet--;
            }
        }
        else
        {
            Debug.Log("������");
            launchable.Bullet--;
        }

        Debug.DrawRay(ray.origin, shootDirection * 100f, Color.red, 2f);
    }

    Vector2 GetRecoilOffset()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomRadius = Random.Range(0f, recoilRadius);
        float x = Mathf.Cos(randomAngle) * randomRadius;
        float y = Mathf.Sin(randomAngle) * randomRadius;

        recoilRadius += recoilIncrease; // �ݵ� ���� ����

        return new Vector2(x, y);
    }

    IEnumerator CountAttackDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
}
