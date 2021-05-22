using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using Cinemachine;

public class PlayerAim : MonoBehaviour
{

    [SerializeField]
    private Rig bodyAimLayer, handAimLayer;
    [SerializeField]
    private Transform weapons; 

    private Animator animator;
    private PlayerInput playerInput;
    private PlayerStats playerStats;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void Aim()
    {
        bool isAiming = playerInput.IsAiming();
        float aimWeight = isAiming ? 1f : 0f;

        animator.SetBool("IsAiming", isAiming);

        bodyAimLayer.weight = aimWeight;
        handAimLayer.weight = aimWeight;
        weapons.gameObject.SetActive(isAiming);
    }
}
