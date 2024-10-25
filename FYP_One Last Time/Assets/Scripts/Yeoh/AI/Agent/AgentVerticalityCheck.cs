using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentVelocity))]

public class AgentVerticalityCheck : MonoBehaviour
{
    AgentVelocity agentV;

    void Awake()
    {
        agentV = GetComponent<AgentVelocity>();
    }  

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        StartCoroutine(CheckingHeight());
    }

    // ============================================================================
    
    public float checkHeightInterval=.15f;

    IEnumerator CheckingHeight()
    {
        while(true)
        {
            yield return new WaitForSeconds(checkHeightInterval);
            CheckHeight(agentV.goal.position);
        }
    }

    // ============================================================================
    
    public float checkingRange=6;

    bool InCheckingRange(Vector3 pos)
    {
        float distance = Vector3.Distance(pos, transform.position);
        return distance <= checkingRange;
    }

    // ============================================================================
    
    public float verticalCheckHeight=3;

    void CheckHeight(Vector3 target_pos)
    {
        if(!agentV.goal) return;

        if(!InCheckingRange(target_pos)) return;

        float target_height = target_pos.y - transform.position.y;

        // is above
        if(target_height > verticalCheckHeight)
        {
            EventM.OnAgentTryJump(gameObject, 1); // jump duh
            EventM.OnAgentTryMove(gameObject, Vector2.up); // press up
        }
        // is below
        else if(target_height < -verticalCheckHeight)
        {
            EventM.OnAgentTryJump(gameObject, 0); // jumpcut
            EventM.OnAgentTryMove(gameObject, Vector2.down); // press down
        }
    }    

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos=true;
    public Color gizmoColor = new(0,1,0,.25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, checkingRange);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * verticalCheckHeight);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * verticalCheckHeight);
    }
}
