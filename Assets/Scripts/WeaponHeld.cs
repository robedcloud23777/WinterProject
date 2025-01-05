using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHeld : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    private Weapon _holdingWeapon;

    public void HoldWeapon(int index)
    {
        ClearHand();
        if (index >= weapons.Count) return;
        _holdingWeapon = Instantiate(weapons[index], handTransform);
    }

    private void ClearHand()
    {
        if(_holdingWeapon == null) return;
        Destroy(_holdingWeapon.gameObject);
            _holdingWeapon = null;
    }
}