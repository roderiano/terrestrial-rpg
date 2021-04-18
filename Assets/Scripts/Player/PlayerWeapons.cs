using UnityEngine.Animations.Rigging;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{   
    [SerializeField]
    private Transform weapons;
    [SerializeField]
    private TwoBoneIKConstraint leftArmIK, rightArmIK; 

    private Transform refLeftHand, refRightHand;

    void Update()
    {
        if(refLeftHand != null && refRightHand != null)
        {
            leftArmIK.data.target.position = refLeftHand.position;
            leftArmIK.data.target.rotation = refLeftHand.rotation;
            rightArmIK.data.target.position = refRightHand.position;
            rightArmIK.data.target.rotation = refRightHand.rotation;
        }
        
    }
    
    public void SetFireGun(Item item)
    {
        // Set FireGun
        bool active;        
        foreach (Transform weapon in weapons)
        {
            active = weapon.name == item.id ? true : false;
            weapon.gameObject.SetActive(active);

            if(active) 
            {
                refLeftHand = weapon.Find("RefLeftHand");
                refRightHand = weapon.Find("RefRightHand");
            }
        }
    }
}
