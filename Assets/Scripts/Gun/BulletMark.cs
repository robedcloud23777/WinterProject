using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMark : MonoBehaviourPun
{
    public PlayerController playerController;
    [SerializeField] private GameObject sparkVFX;
    [SerializeField] private GameObject markVFX;
    [SerializeField] private GameObject buffMarkVFX;

    // 총구 스파크 생성 (위치만 동기화)
    public void Spark(Transform firePoint)
    {
        photonView.RPC("SparkInstantiate", RpcTarget.All, firePoint.position);
    }

    // 총알 자국 생성 (위치 및 방향 동기화)
    public void MakeMark(RaycastHit hit)
    {
        if (playerController.isDamageBuff)
            photonView.RPC("MakeMarkInstantiate", RpcTarget.All, buffMarkVFX.name, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
        else
            photonView.RPC("MakeMarkInstantiate", RpcTarget.All, markVFX.name, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
    }

    // RPC: 총구 스파크 생성 (회전값 없이 기본 회전 유지)
    [PunRPC]
    private void SparkInstantiate(Vector3 position)
    {
        Instantiate(sparkVFX, position, Quaternion.identity);
    }

    // RPC: 총알 자국 생성 (표면 방향 적용)
    [PunRPC]
    private void MakeMarkInstantiate(string vfxName, Vector3 position, Quaternion rotation)
    {
        GameObject vfxPrefab = Resources.Load<GameObject>(vfxName);
        Instantiate(vfxPrefab, position, rotation);
    }
}
