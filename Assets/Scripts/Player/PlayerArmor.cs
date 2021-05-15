using UnityEngine;

public class PlayerArmor : MonoBehaviour
{   

    [SerializeField]
    private Transform headArmors, chestArmors, LegsArmors;

    public void SetHead(Item item)
    {
        // Active primary weapon
        bool active;        
        foreach (Transform armor in headArmors)
        {
            if(item != null)
                active = armor.name == item.id ? true : false;
            else
                active = false;

            armor.gameObject.SetActive(active);
        }
    }

    public void SetChest(Item item)
    {
        // Active primary weapon
        bool active;        
        foreach (Transform armor in chestArmors)
        {
            if(item != null)
                active = armor.name == item.id ? true : false;
            else
                active = false;

            armor.gameObject.SetActive(active);
        }
    }

    public void SetLegs(Item item)
    {
        // Active primary weapon
        bool active;        
        foreach (Transform armor in LegsArmors)
        {
            if(item != null)
                active = armor.name == item.id ? true : false;
            else
                active = false;

            armor.gameObject.SetActive(active);
        }
    }
}
