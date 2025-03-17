using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAgent : MonoBehaviour
{
    public GameObject owner;
    public Pilot pilot;

    // ============================================================================

    EnemyManager EnemyM;
    protected EventManager EventM;

    void OnEnable()
    {
        EnemyM = EnemyManager.Current;
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
    public SideFlip flip;
    public Vector2 seekFlipDelay = new(.4f, .6f); 

    // ============================================================================

    [Header("For Enemy Manager")]
    public int combatSlotNum = 0;

    public void RegisterEnemy() => EnemyM?.RegisterEnemy(owner);
    public void UnregisterEnemy() => EnemyM?.UnregisterEnemy(owner);

    public void RegisterEnemyCombat() => EnemyM?.RegisterEnemyCombat(owner, combatSlotNum);
    public void UnregisterEnemyCombat() => EnemyM?.UnregisterEnemyCombat(owner);

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnSeekEnter;
        public UnityEvent OnSeekExit;
    };
    [Space]
    public Events events;
}
