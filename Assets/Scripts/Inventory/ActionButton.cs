using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum MenuAction {
    Use,
    Drop,
    Back,
    Equip,
    Unequip
}


public class ActionButton : MonoBehaviour
{
    private TextMeshProUGUI text;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Set <paramref name="action"/> of ActionButton and refresh the text.
    /// </summary>
    /// /// <param name="action">Action of ActionButton.</param>
    public void SetAction(MenuAction action)
    {
        switch (action)
        {
            case(MenuAction.Equip):
                text.SetText("Equip");
                break;
            case(MenuAction.Unequip):
                text.SetText("Unequip");
                break;
            case(MenuAction.Back):
                text.SetText("Back");
                break;
            case(MenuAction.Drop):
                text.SetText("Drop");
                break;
        }
    }
}
