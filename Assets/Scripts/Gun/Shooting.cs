using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Camera mainCamera;
    public Transform firePoint; // �ѱ� ��ġ
    public float delayTime = 0.1f; // ���� �ӵ� ����
    public float recoilResetTime = 0.5f; // �ݵ� �ʱ�ȭ �ð�
    public float recoilRadius = 0.1f; // �ݵ� �� ũ��
    public float recoilIncrease = 0.005f; // �ݵ� ������
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

        // ���� �ð� ������ �ݵ� ���� �ʱ�ȭ
        if (Time.time - lastShotTime > recoilResetTime)
        {
            shootIndex = 0;
            recoilRadius = 0.1f; // �ݵ� �ʱ�ȭ
        }

        if (playerInput.GetRInput() && launchable.bullet != 25)
            launchable.Reload();
    }

    private void FireRaycast()
    {
        lastShotTime = Time.time;

        // ȭ�� �߾ӿ��� �߻� ������ ����
        Ray screenRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        // �ѱ� ��ġ���� ȭ�� �߾� �������� Ray �߻�
        Vector2 recoilOffset = GetRecoilOffset();
        Vector3 shootDirection = screenRay.direction + mainCamera.transform.right * recoilOffset.x + mainCamera.transform.up * recoilOffset.y;
        shootDirection.Normalize();

        Ray ray = new Ray(firePoint.position, shootDirection);
        RaycastHit hit;

        // �ѱ����� ����ũ VFX ����
        //Instantiate(bulletMark.sparkVFX, firePoint.position, firePoint.rotation);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("���� ��ü�� ���̾�: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            string hitLayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            launchable.bullet--;

            if (hitLayerName == "Player")
            {
                // ������ ���� �߰� ����
                launchable.bullet--;
            }
            else if (hitLayerName == "map")
            {
                // �Ѿ� �ڱ� ȿ�� ����
                //bulletMark.MakeMark(hit.point, hit.normal);
                launchable.bullet--;
            }
        }
        else
        {
            Debug.Log("������");
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

        recoilRadius += recoilIncrease; // �ݵ� ���� ����

        return new Vector2(x, y);
    }

    IEnumerator CountAttackDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
}
