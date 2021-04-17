using UnityEngine;

public class PlayerInstanceHandler : MonoBehaviour
{   
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        playerInput.CaptureInputs();
    }

    private void FixedUpdate()
    {
        playerMovement.Move();
    }
}
