using UnityEngine;
using Cinemachine;

public class PlayerInstanceHandler : MonoBehaviour
{   
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void FixedUpdate()
    {
        playerMovement.Movement();
    }
}
