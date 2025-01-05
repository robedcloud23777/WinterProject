using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun, IZoomable
{
    protected override void MouseLeftPress()
    {
        base.MouseLeftPress();
        Attack();
    }

    protected override void RKeyPress()
    {
        base.RKeyPress();
        Reload();
    }

    public void ZoomIn()
    {
        
    }

    public void ZoomOut()
    {
        
    }
}
