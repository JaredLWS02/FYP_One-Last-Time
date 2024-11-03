using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetDeviceBindings : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string targetControlScheme;

    public void ResetAllBindings()
    {
        foreach(var map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }

    public void ResetControlSchemeBinding()
    {
        foreach(var map in inputActions.actionMaps)
        {
            foreach(var action in map.actions)
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(targetControlScheme));
            }
        }
    }
}
