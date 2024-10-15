using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }
        
    // ============================================================================

    public int players;

    // Input System ============================================================================

    void OnPlayerJoined(PlayerInput input)
    {
        foreach(var device in input.devices)
        {
            if(IsDeviceIncluded(device))
            {
                players++;

                Debug.Log($"Player {players} joined with {device.displayName}");
            }
        }
    }

    void OnPlayerLeft(PlayerInput input)
    {
        foreach(var device in input.devices)
        {
            if(IsDeviceIncluded(device))
            {
                Debug.Log($"Player {players} left with {device.displayName}");

                players--;
            }
        }
    }

    // ============================================================================

    bool IsDeviceIncluded(InputDevice device)
    {
        return device is Keyboard || device is Gamepad || device is Joystick;
    }
}
