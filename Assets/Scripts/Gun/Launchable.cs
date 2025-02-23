using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchable : MonoBehaviour
{
    public int Bullet = 30;

    public bool IsShoot()
    {
        if (Bullet > 0)
        {
            return true;
        }
        else
        {
            StartCoroutine(Reload());
            return false;
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(3f);
        Bullet = 30;
    }
}
