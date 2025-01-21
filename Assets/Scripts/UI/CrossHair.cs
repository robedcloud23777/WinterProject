using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public Transform crosshair;

    void Update()
    {
        
        Vector3 mousePosition = Input.mousePosition;

        crosshair.position = mousePosition;
    }
}
