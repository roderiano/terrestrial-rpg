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
            active = armor.name == item.id ? true : false;
            armor.gameObject.SetActive(active);
        }
    }

    public void SetChest(Item item)
    {
        // Active primary weapon
        bool active;        
        foreach (Transform armor in chestArmors)
        {
            active = armor.name == item.id ? true : false;
            armor.gameObject.SetActive(active);
        }
    }

    public void SetLegs(Item item)
    {
        // Active primary weapon
        bool active;        
        foreach (Transform armor in LegsArmors)
        {
            active = armor.name == item.id ? true : false;
            armor.gameObject.SetActive(active);
        }
    }
}
