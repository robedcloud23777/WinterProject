using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun, IShakeable 
{
    [SerializeField] private float recoilForce;
    
    protected override void MouseLeftPress()
    {
        base.MouseLeftPress();
        if (Attack())
        {
            Shake(recoilForce);
        }
    }

    protected override void RKeyPress()
    {
        base.RKeyPress();
        Reload();
    }

    public void Shake(float force)
    {
        
    }
}
