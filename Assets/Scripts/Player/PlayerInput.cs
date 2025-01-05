using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 GetMoveInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(x, y);
        return input.normalized;
    }

    public bool GetJumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public int GetNumberInput()
    {
        for (int i = 0; i < 9; ++i)
        {
            int index = i;
            if(Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                return index;
            }
        }
        return -1;
    }
}
