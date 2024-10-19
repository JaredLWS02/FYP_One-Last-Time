using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentVelocity))]

public class JumpAI : MonoBehaviour
{
    AgentVelocity seek;

    void Awake()
    {
        seek = GetComponent<AgentVelocity>();
    }
    
    // ============================================================================

    public Vector3 range = new(10, 15, 10);

    void FixedUpdate()
    {
        if(!seek.goal) return;

        Vector3 goal_pos = seek.goal.position;

        if(!InRange(goal_pos)) return;

        CheckHeight(goal_pos);
    }

    bool InRange(Vector3 pos)
    {
        float x_distance = Mathf.Abs(pos.x - transform.position.x);
        float y_distance = Mathf.Abs(pos.y - transform.position.y);
        float z_distance = Mathf.Abs(pos.z - transform.position.z);

        return x_distance <= range.x &&
            y_distance <= range.y &&
            z_distance <= range.z;
    }

    void CheckHeight(Vector3 target)
    {
        float target_height = target.y - transform.position.y;
        float stopping_range = seek.stoppingRange;

        // is above
        if(target_height > stopping_range)
        {
            //EventManager.Current.OnTryJump(gameObject, 1); // jump duh
            //EventManager.Current.OnTryMoveY(gameObject, 1); // press up
        }
        // is below
        else if(target_height < -stopping_range)
        {
            //EventManager.Current.OnTryJump(gameObject, 0); // jumpcut
            //EventManager.Current.OnTryMoveY(gameObject, -1); // press down
        }
    }

    // ============================================================================

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new(1,1,1,.5f);
        Gizmos.DrawWireCube(transform.position, range);
    }
}
