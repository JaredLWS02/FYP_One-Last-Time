using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

[System.Serializable]
public class RangeAssistCfg
{
    public float moveSeconds=.2f;
    public float maxMoveDistance=3;
    public bool alwaysMove=true;
    public Ease ease = Ease.OutSine;
    public float reach=2.5f;
};

// ============================================================================

public class RangeAssist : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;
    public BaseRaycast ray;
    
    public RangeAssistCfg defaultCfg;

    public void CheckRange(RangeAssistCfg cfg)
    {
        ray.Shoot();

        if(!ray.IsHitting())
        {
            TryMoveAnyways(cfg);
            return;
        }

        float max_dist = cfg.maxMoveDistance + cfg.reach;

        if(ray.rayHit.distance > max_dist)
        {
            TryMoveAnyways(cfg);
            return;
        }

        Vector3 reach_point = ray.rayHit.point + (-ray.origin.forward * cfg.reach);

        float ownerY = owner.transform.position.y; // feet level
        float rayhitY = ray.rayHit.point.y; // hit.point height
        // difference between them
        float offsetY = ownerY - rayhitY; 
        // offset the reach point
        reach_point.y += offsetY;

        TweenPos(reach_point, cfg);
    }

    public void CheckRange() => CheckRange(defaultCfg);

    // ============================================================================

    void TryMoveAnyways(RangeAssistCfg cfg)
    {
        if(!cfg.alwaysMove) return;

        Vector3 max_move_point = owner.transform.position + owner.transform.forward * cfg.maxMoveDistance;
        TweenPos(max_move_point, cfg);
    }

    // ============================================================================

    [Space]
    public LayerMask obstacleLayers;

    Tween posTween;

    void TweenPos(Vector3 to, RangeAssistCfg cfg)
    {
        if(rb) rb.velocity = Vector3.zero;

        if (owner.transform.position == to) return;
        
        Vector3 move_vector = to - owner.transform.position;
        Vector3 move_direction = move_vector.normalized;
        float move_distance = move_vector.magnitude;

        // if got obstacles in the way, move to hit.point instead
        if(Physics.Raycast(owner.transform.position, move_direction, out RaycastHit hit, move_distance, obstacleLayers, QueryTriggerInteraction.Ignore))
        {
            to = hit.point;
        }

        posTween.Stop();
        if(cfg.moveSeconds>0)
        {
            TweenSettings tweencfg = new(
                duration: cfg.moveSeconds,
                ease: cfg.ease,
                useFixedUpdate: rb ? true : false
            );

            posTween = Tween.Position(owner.transform, to, tweencfg);
        }
        else owner.transform.position = to;
    }

    // ============================================================================
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmo1Color = new(1, 1, 1, .5f);
    public Color gizmo2Color = new(1, 1, 0, .5f);
    public float gizmoSize=.2f;

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!ray.origin) return;

        Vector3 maxMovePos = ray.origin.position + ray.origin.forward * defaultCfg.maxMoveDistance;
        Vector3 reachPos = maxMovePos + ray.origin.forward * defaultCfg.reach;

        Gizmos.color = gizmo1Color;
        Gizmos.DrawSphere(maxMovePos, gizmoSize);
        Gizmos.color = gizmo2Color;
        Gizmos.DrawSphere(reachPos, gizmoSize);
    }
}
