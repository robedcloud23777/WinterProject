using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Camera mainCamera;
    public float delayTime = 0.3f;
    private bool isDelay;

    void Update()
    {
        if (Input.GetMouseButton(0) && !isDelay)
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
            Debug.Log("if 쳐맞은게 플레이어면 데미지 코드 넣으셈");


        }
        else
        {
            Debug.Log("아무것도 안맞음");
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
    }

    IEnumerator CountAttackDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
}