using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilot : MonoBehaviour
{
    public enum Type
    {
        None,
        Player,
        AI,
    }

    public Type type = Type.Player;  

    // Event Manager ============================================================================

    void OnEnable()
    {
        EventManager.Current.SwitchPilotEvent += SwitchPilot;
    }
    void OnDisable()
    {
        EventManager.Current.SwitchPilotEvent -= SwitchPilot;
    }

    // Events ============================================================================

    void SwitchPilot(GameObject who, Type to)
    {
        if(gameObject!=who) return;

        type = to;
    }

    //  ============================================================================

    public bool IsPlayer()
    {
        return type==Type.Player;
    }    

}
