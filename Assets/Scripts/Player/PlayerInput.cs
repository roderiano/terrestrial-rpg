using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool aimLock = false;

    private bool isAiming, inventory;
    private Vector2 mouseAxis;
    private float horizontal, vertical;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public void CaptureInputs() 
    {
        horizontal = Input.GetAxis("Horizontal"); 
        vertical = Input.GetAxis("Vertical");
        mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        isAiming = aimLock ? true : Input.GetMouseButton(1) && playerStats.GetFireGunSlot() != null;
        inventory = Input.GetKeyDown(KeyCode.Tab);
    }

    public float GetHorizontal() 
    {
        return horizontal;
    }

    public float GetVertical() 
    {
        return vertical;
    }

    public Vector2 GetMouseAxis() 
    {
        return mouseAxis;
    }

    public bool IsAiming() 
    {
        return isAiming;
    }

    public bool Inventory() 
    {
        return inventory;
    }
}
