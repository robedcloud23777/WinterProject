using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 GetMoveInput()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return Vector2.zero;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(x, y);
        return input.normalized;
    }

    public bool GetJumpInput()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return false;
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool GetCrouchInput()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return false;
        return Input.GetKey(KeyCode.LeftControl);
    }

    public bool GetRunInput()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return false;
        return Input.GetKey(KeyCode.LeftShift);
    }

    public bool GetShootInput()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return false;
        return Input.GetMouseButton(0);
    }

    public bool GetZoomInput()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return false;
        return Input.GetMouseButton(1);
    }

    public bool GetQInput()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return false;
        return Input.GetKeyDown(KeyCode.Q);
    }

    public bool GetEInput()
    {
        if (GameManager.Instance.settingPanelInstance.activeSelf) return false;
        return Input.GetKeyDown(KeyCode.E);
    }
}
