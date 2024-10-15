using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SideMove))]

public class AISideMove : MonoBehaviour
{
    SideMove move;

    void Awake()
    {
        move = GetComponent<SideMove>();
    }

    public void Move()
    {
        move.dirX = GetMoveDir();
    }

    // ============================================================================

    public Transform target;
    
    [Header("Seek")]
    public bool arrival=true;
    public float stoppingRange=.05f;
    
    [Header("Flee")]
    public bool flee;
    public bool departure=true;
    public float fleeRange=4;

    [Header("Slowing")]
    public float slowingRangeOffset=.5f;

    // ============================================================================

    float GetMoveDir()
    {
        if(!target) return 0;

        float dir = flee ?
            GetFleeDir():
            GetSeekDir();

        return dir;
    }

    float GetSeekDir()
    {
        float max_speed = 1;
        float speed;

        if(arrival)
        {
            float distance = Mathf.Abs(target.position.x - transform.position.x);

            if(distance <= stoppingRange)
            {
                speed=0;
            }
            else
            {
                float ramped = max_speed * distance / (stoppingRange+slowingRangeOffset);

                float clipped = Mathf.Min(ramped, max_speed);

                speed = clipped;
            }
        }
        else speed = max_speed;

        return target.position.x >= transform.position.x ? speed : -speed;
    }

    float GetFleeDir()
    {
        float max_speed = 1;
        float speed;

        if(departure)
        {
            float distance = Mathf.Abs(target.position.x - transform.position.x);
            
            if(distance <= fleeRange)
            {
                if(distance <= fleeRange-slowingRangeOffset)
                {
                    speed = max_speed;
                }
                else
                {
                    float ramped = max_speed * (fleeRange-distance) / slowingRangeOffset;

                    float clipped = Mathf.Min(ramped, max_speed);

                    speed = clipped;
                }
            }
            else speed=0;
        }
        else speed = max_speed;

        return target.position.x >= transform.position.x ? -speed : speed;
    }

    // ============================================================================

    public Vector3 GetDir(Vector3 to, Vector3 from)
    {
        return (to-from).normalized;
    }
}
