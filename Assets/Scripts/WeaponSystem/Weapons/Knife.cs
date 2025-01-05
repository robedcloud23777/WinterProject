using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon, IAttack
{
    protected override void MouseLeftPress()
    {
        base.MouseLeftPress();
        Attack();
    }

    public bool Attack()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, detectLayer))
        {
            hit.transform.TryGetComponent<IDamageable>(out IDamageable damageable);
            damageable.TakeDamage(weaponDamage);
            return true;
        }
        return false;
    }
}
