using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Gun, IShakeable, IZoomable
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

    protected override void MouseRightPress()
    {
        base.MouseRightPress();
        ZoomIn();
    }

    protected override void MouseRightRelease()
    {
        base.MouseRightRelease();
        ZoomOut();
    }

    public void Shake(float force)
    {
        
    }

    public void ZoomIn()
    {
        
    }

    public void ZoomOut()
    {
        
    }
}
