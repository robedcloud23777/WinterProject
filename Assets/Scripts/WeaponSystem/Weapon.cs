using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int weaponDamage;
    [SerializeField] protected float maxDistance;
    [SerializeField] protected LayerMask detectLayer;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MouseLeftPress();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            MouseLeftRelease();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            MouseRightPress();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            MouseRightRelease();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            RKeyPress();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            RKeyRelease();
        }
    }

    protected virtual void MouseLeftPress() { }
    protected virtual void MouseLeftRelease() { }
    protected virtual void MouseRightPress() { }
    protected virtual void MouseRightRelease() { }
    protected virtual void RKeyPress() { }
    protected virtual void RKeyRelease() { }
}
