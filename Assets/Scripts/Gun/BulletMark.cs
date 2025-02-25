using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMark : MonoBehaviour
{
    [SerializeField] private GameObject sparkVFX;
    [SerializeField] private GameObject markVFX;

    public void Spark(Transform firePoint)
    {
        Instantiate(sparkVFX, firePoint.position, firePoint.rotation);
    }

    public void MakeMark(RaycastHit hit) 
    {
        Instantiate(markVFX, hit.point, Quaternion.LookRotation(hit.normal));
        Debug.Log("발사 파티클 온");
    }
}
