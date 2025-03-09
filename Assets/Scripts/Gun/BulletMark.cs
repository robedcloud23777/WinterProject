using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMark : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField] private GameObject sparkVFX;
    [SerializeField] private GameObject markVFX;
    [SerializeField] private GameObject buffMarkVFX;

    public void Spark(Transform firePoint)
    {
        GameObject muzzleFlash = Instantiate(sparkVFX, firePoint.position, firePoint.rotation * Quaternion.Euler(0, -90, -90));
        Destroy(muzzleFlash, 1.0f);
    }

    public void MakeMark(RaycastHit hit) 
    {
        GameObject muzzleFlash;
        if (!playerController.isDamageBuff) muzzleFlash = Instantiate(markVFX, hit.point, Quaternion.LookRotation(hit.normal));
        else muzzleFlash = Instantiate(buffMarkVFX, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(muzzleFlash, 1.0f);
    }
}
