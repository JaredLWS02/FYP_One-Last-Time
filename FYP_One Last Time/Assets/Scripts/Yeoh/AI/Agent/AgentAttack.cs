using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    public string attackName = "Normal Combo";

    public void Attack()
    {
        EventM.OnAgentTryAttack(owner, attackName);
    }
}
