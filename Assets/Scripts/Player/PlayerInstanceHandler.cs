using UnityEngine;
using Cinemachine;

public class PlayerInstanceHandler : MonoBehaviour
{   
    private PlayerMovement playerMovement;
    private PlayerAim playerAim;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAim = GetComponent<PlayerAim>();
    }

    private void Update()
    {
        playerAim.Aim();
    }

    private void FixedUpdate()
    {
        playerMovement.Movement();
    }
}
