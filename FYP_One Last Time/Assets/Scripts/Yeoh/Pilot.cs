using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PilotType
{
    None,
    Player,
    AI,
}

public class Pilot : MonoBehaviour
{
    public PilotType currentPilot = PilotType.Player;  

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.SwitchPilotEvent += SwitchPilot;
    }
    void OnDisable()
    {
        EventM.SwitchPilotEvent -= SwitchPilot;
    }

    // ============================================================================

    void SwitchPilot(GameObject who, PilotType to)
    {
        if(gameObject!=who) return;

        currentPilot = to;
    }

    //  ============================================================================

    public bool IsType(PilotType type)
    {
        return currentPilot==type;
    }
    public bool IsNone()
    {
        return IsType(PilotType.None);
    }    
    public bool IsPlayer()
    {
        return IsType(PilotType.Player);
    }    
    public bool IsAI()
    {
        return IsType(PilotType.AI);
    }    
    
}
