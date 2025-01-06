using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 GetMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 movevector2 = new Vector2(h, y).normalized;
        return movevector2;
    }



    public bool GetJumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    
    
}
