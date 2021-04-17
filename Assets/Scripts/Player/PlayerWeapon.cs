using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private Weapon activeWeapon;
    
    [SerializeField]
    private Transform weapons;
    [SerializeField]
    private TwoBoneIKConstraint leftArmIK, rightArmIK; 
    
    void FixedUpdate()
    {
        // Executar essa rotina somente uma vez 
        // depois que tiver um trigger para ativar as armas
        // Setar posição do armIK da arma no PlayerAim 
        bool active;
        Transform refLeftHand, refRightHand;
        foreach (Transform weapon in weapons)
        {
            active = weapon.name == activeWeapon.id ? true : false;
            weapon.gameObject.SetActive(active);

            if(active) 
            {
                refLeftHand = weapon.Find("RefLeftHand");
                leftArmIK.data.target.position = refLeftHand.position;
                leftArmIK.data.target.rotation = refLeftHand.rotation;

                refRightHand = weapon.Find("RefRightHand");
                rightArmIK.data.target.position = refRightHand.position;
                rightArmIK.data.target.rotation = refRightHand.rotation;
            }
        }
    }
}
