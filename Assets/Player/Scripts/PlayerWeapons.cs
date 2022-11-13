using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{   
    [SerializeField]
    private Transform weapons;
    [SerializeField]
    private TwoBoneIKConstraint leftArmIK, rightArmIK; 
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform aimPointTransform;
    [SerializeField]
    private LayerMask aimMask;

    private Transform refLeftHand, refRightHand;
    private Slot fireGunSlot;
    private GameObject firegun;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SetAimPointPosition();

        if(GetFireGunSlot() != null)
        {
            if(Input.GetKey(KeyCode.Mouse1))
            {
                firegun.transform.parent.localPosition = new Vector3(1f, 5.2f, 0f);
                firegun.transform.parent.LookAt(aimPointTransform);
            }
            else
            {
                firegun.transform.parent.localPosition = new Vector3(1f, 5.2f, 1f);
                firegun.transform.parent.localEulerAngles = new Vector3(30f, -80f, 0f);
            }
        }
    }

    private void SetAimPointPosition() 
    {
        RaycastHit hit;

        if(
            Physics.Raycast(
                cameraTransform.position, 
                cameraTransform.TransformDirection(Vector3.forward), 
                out hit, Mathf.Infinity, aimMask
            )
        )
        {
            aimPointTransform.position = hit.point;
        }
        else
        {
            aimPointTransform.position = cameraTransform.position + (cameraTransform.forward * 1000);
        }


    }
    
    private void EnableFireGun(Slot slot)
    {
        // Set FireGun
        bool active;        
        foreach (Transform weapon in weapons)
        {
            if(slot != null)
                active = weapon.name == slot.item.id ? true : false;
            else
                active = false;
            
            weapon.gameObject.SetActive(active);

            if(active) 
            {
                firegun = weapon.gameObject;
                refLeftHand = weapon.Find("RefLeftHand");
                refRightHand = weapon.Find("RefRightHand");
            }
        }
    }

    /// <summary>
    /// Get FireGun Slot.
    /// </summary>
    /// <returns>
    /// The equiped FireGun Slot.
    /// </returns>
    public Slot GetFireGunSlot() 
    {
        return fireGunSlot;
    }

    /// <summary>
    /// Set FireGun Slot.
    /// </summary>
    /// <param name="slot">Slot of FireGun to be equiped.</param>
    public void SetFireGunSlot(Slot slot) 
    {
        fireGunSlot = slot;
        EnableFireGun(slot);
    }

    /// <summary>
    /// Get FireGun damage.
    /// </summary>
    /// <returns>
    /// The dictionary of FireGun damage.
    /// </returns>
    public Dictionary<string, float> GetFireGunDamage() 
    {
        string[] resistanceKeys = {"physical", "frost", "fire", "magical", "decay"};
        Dictionary<string, float> damageDictionary = new Dictionary<string, float>();

        foreach (string key in resistanceKeys)
        {
            float value = fireGunSlot != null ? (float)fireGunSlot.item.GetType().GetField(key).GetValue(fireGunSlot.item) : 0f;
            damageDictionary.Add(key, value);
        }

        return damageDictionary;
    }

    private void OnAnimatorIK()
    {
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(aimPointTransform.position);

        if(refRightHand != null && refLeftHand != null)
        {
            if(Input.GetKey(KeyCode.Mouse1) && GetFireGunSlot() != null || true)
            {
                
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                
                animator.SetIKPosition(AvatarIKGoal.LeftHand, refLeftHand.position);
                animator.SetIKPosition(AvatarIKGoal.RightHand, refRightHand.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, refLeftHand.rotation);
                animator.SetIKRotation(AvatarIKGoal.RightHand, refRightHand.rotation);
            }
        }
        else 
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
    }
}
