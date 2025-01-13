using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Camera mainCamera;
    public float delayTime = 0.3f;
    private bool isDelay ;

    void Update()
    {
        if (Input.GetMouseButton(0)&&!isDelay)
        {
            isDelay = true;
            FireRaycast();
            StartCoroutine(CountAttackDelay());
        }
    }

    void FireRaycast()
    {


        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"Hit {hit.collider.gameObject.name} at {hit.point}");


        }
        else
        {
            Debug.Log("No hit detected.");
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
    }

    IEnumerator CountAttackDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
}
