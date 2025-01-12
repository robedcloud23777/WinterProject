using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireRaycast();
        }
    }

    void FireRaycast()
    {


        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"Hit {hit.collider.gameObject.name} at {hit.point}");


        }
        else
        {
            Debug.Log("No hit detected.");
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
    }
}
