using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISideSeek : MonoBehaviour
{
    [HideInInspector]
    public Transform goal;
    [HideInInspector]
    public Vector3 seekPos;

    [Header("Arrival")]
    public bool arrival=true;
    public float stoppingRange=2;
    public float slowingRangeOffset=2;

    // ============================================================================

    public void Move()
    {
        EventManager.Current.OnTryMoveX(gameObject, GetSeekInput());
    }

    float GetSeekInput()
    {
        if(!goal) return 0;

        float input_x;
        float max_input=1;

        if(arrival)
        {
            float distance = Mathf.Abs(goal.position.x - transform.position.x);

            if(distance <= stoppingRange)
            {
                input_x=0;
            }
            else
            {
                float ramped = max_input * distance / (stoppingRange+slowingRangeOffset);

                float clipped = Mathf.Min(ramped, max_input);

                input_x = clipped;
            }
        }
        else input_x = max_input;

        return seekPos.x >= transform.position.x ? input_x : -input_x;
    }
}
