using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchable : MonoBehaviour
{
    public int bullet = 25;

    public bool IsShoot()
    {
        if (bullet > 0)
        {
            return true;
        }
        else return false;
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(2.5f);
        bullet = 25;
    }
}
