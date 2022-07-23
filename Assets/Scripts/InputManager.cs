using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    private static readonly KeyCode jumpCode = KeyCode.W;
    private static readonly KeyCode dashCode = KeyCode.LeftShift;
    private static readonly int fireKey = 0;

    public static Vector2 MovementInput(){
        return new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")); 
    }

    public static bool JumpButtonPressed(){
        return Input.GetKeyDown(jumpCode);
    }

    public static bool FireKeyPressed()
    {
        return Input.GetMouseButton(fireKey);
    }

    public static bool DashKeyPressed(){
        return Input.GetKey(dashCode);
    }
}
