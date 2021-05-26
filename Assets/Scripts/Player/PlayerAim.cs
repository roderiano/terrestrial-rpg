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

    private bool active;
    private Animator animator;
    private PlayerStats playerStats;

    void Start()
    {
        active = true;
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void Aim()
    {
        if(active)
        {
            bool isAiming = (Input.GetButton("Aim") || Input.GetAxis("Aim") != 0f) && playerStats.GetFireGunSlot() != null;
            float aimWeight = isAiming ? 1f : 0f;

            animator.SetBool("IsAiming", isAiming);

            bodyAimLayer.weight = aimWeight;
            handAimLayer.weight = aimWeight;
            weapons.gameObject.SetActive(isAiming);
        }
    }

    public void SetActive(bool value) 
    {
        active = value;
        Object.FindObjectOfType<CinemachineFreeLook>().enabled = value;
    }
}
