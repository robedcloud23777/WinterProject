using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraController cameraController;
   
    public void Update()
    {
        playerMovement.MoveByInput(playerInput.GetMoveInput());
        if (playerInput.GetJumpInput())
        {
            playerMovement.JumpByInput();
        }
        playerMovement.ImplementGravity();

    
    }
}
