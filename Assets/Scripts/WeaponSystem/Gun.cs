using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gun : Weapon, IAttack, IReloadable
{
    [SerializeField] private int maxAmmoCount = 8;
    [SerializeField] private int curAmmoCount;
    [SerializeField] private int totalAmmoCount;
    [SerializeField] private int ammoPerShot = 1;
    
    public virtual bool Attack()
    {
        if (curAmmoCount <= 0)
        {
            return false; // 총알이 없으면 공격 실패
        }

        // 사용 가능한 총알 계산
        int shootingAmmoCount = Mathf.Min(ammoPerShot, curAmmoCount);
        curAmmoCount -= shootingAmmoCount;

        // 레이캐스트로 타격 처리
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, detectLayer))
        {
            if (hit.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(weaponDamage * shootingAmmoCount);
            }
        }

        return true; // 공격 성공
    }

    public virtual void Reload()
    {
        int neededCount = maxAmmoCount - curAmmoCount; // 최대 총알 수를 채우기 위해 필요한 총알 수

        // 인벤토리에 충분한 총알이 있는 경우
        if (totalAmmoCount >= neededCount)
        {
            curAmmoCount += neededCount;  // 현재 총알을 최대치로 채움
            totalAmmoCount -= neededCount; // 인벤토리에서 총알 차감
        }
        else
        {
            // 인벤토리에 총알이 부족한 경우
            curAmmoCount += totalAmmoCount; // 인벤토리에 남은 총알만큼 현재 총알을 채움
            totalAmmoCount = 0;              // 인벤토리 총알은 모두 사용됨
        }
    }
}
