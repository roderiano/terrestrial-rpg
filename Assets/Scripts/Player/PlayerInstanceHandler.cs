using UnityEngine;
using Cinemachine;

public class PlayerInstanceHandler : MonoBehaviour
{   
    public bool canMove = true;
    
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerAim playerAim;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAim = GetComponent<PlayerAim>();
    }

    private void Update()
    {
        playerInput.CaptureInputs();
        Object.FindObjectOfType<CinemachineFreeLook>().enabled = canMove;

        if(canMove)
        {
            playerAim.Aim();
        }
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            playerMovement.Move();
        }
    }
}
