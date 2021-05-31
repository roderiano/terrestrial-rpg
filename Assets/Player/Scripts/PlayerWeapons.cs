using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{   
    [SerializeField]
    private Transform weapons;
    [SerializeField]
    private TwoBoneIKConstraint leftArmIK, rightArmIK; 

    private Transform refLeftHand, refRightHand;
    private Slot fireGunSlot;

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
}
