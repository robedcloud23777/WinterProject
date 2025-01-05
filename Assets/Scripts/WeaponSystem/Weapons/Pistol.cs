using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
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
}
