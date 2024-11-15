using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    public GameObject owner;
    public Pilot pilot;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.OnSpawned(owner);
    }
    
    // ============================================================================

    public AgentVehicle vehicle;
    public AgentSideMove move;
    public AgentWander wander;
    public AgentTargeting targeting;
    public AgentFlee flee;
    public AgentReturn returner;
    
    // ============================================================================
    
    [Header("Seek")]
    public RandomPicker randomSeekBehaviour;
    
    // ============================================================================
    
    [Header("Flee")]
    public HPManager hpM;
    public float fleeHPPercent=25;
    public bool ShouldFlee() => hpM.GetHPPercent() <= fleeHPPercent;

    public RandomPicker randomFleeBehaviour;
}
