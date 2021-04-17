using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool isAiming;
    private Vector2 mouseAxis;
    private float horizontal, vertical;

    public void CaptureInputs() 
    {
        horizontal = Input.GetAxis("Horizontal"); 
        vertical = Input.GetAxis("Vertical");
        mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        isAiming = Input.GetMouseButton(1);
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
}
