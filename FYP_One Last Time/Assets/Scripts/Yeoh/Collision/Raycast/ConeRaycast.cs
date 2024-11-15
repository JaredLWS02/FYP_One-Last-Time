using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeRaycast : BaseRaycast
{
    public override bool RayHit(out GameObject target)
    {
        foreach(var dir in GetConeDirs(GetRayDir()))
        {
            if(Physics.Raycast(origin.position, dir, out rayHit, range, hitLayers, QueryTriggerInteraction.Ignore))
            {
                if(IsHitValid(out var hitObj))
                {
                    target = hitObj;
                    return true;
                }
            }
        }
        target=null;
        return false;
    }

    // ============================================================================

    [Header("Cone Cast")]
    public Vector2 fov = new(90,90);
    public Vector2Int rays = new(10,10);

    List<Vector3> GetConeDirs(Vector3 main_dir)
    {
        List<Vector3> dirs = new();

        if (rays.x <= 0 || rays.y <= 0) return dirs;

        Vector2 half_fov = fov*.5f;

        bool doHorizontal = fov.x > 0;
        bool doVertical = fov.y > 0;

        Vector2 step = new(
            doHorizontal ? fov.x / rays.x : 0,
            doVertical ? fov.y / rays.y : 0
        );

        // vertical sweep
        for(float i = doVertical ? -half_fov.y:0; i <= (doVertical ? half_fov.y:0); i += doVertical ? step.y : float.MaxValue)
        {
            // horizontal sweep
            for(float j = doHorizontal ? -half_fov.x:0; j <= (doHorizontal ? half_fov.x:0); j += doHorizontal ? step.x : float.MaxValue)
            {
                // Convert polar coordinates (pitch, yaw) to direction
                float radPitch = Mathf.Deg2Rad * i; // Convert pitch to radians
                float radYaw = Mathf.Deg2Rad * j;   // Convert yaw to radians

                // Spherical coordinates to cartesian
                float x = Mathf.Cos(radPitch) * Mathf.Sin(radYaw);
                float y = Mathf.Sin(radPitch);
                float z = Mathf.Cos(radPitch) * Mathf.Cos(radYaw);

                Vector3 local_dir = new Vector3(x, y, z).normalized;

                // Apply world rotation based on the ray's rotation
                Vector3 dir = Quaternion.LookRotation(main_dir) * local_dir;
                
                dirs.Add(dir);
            }
        }

        dirs.Add(main_dir); // center ray

        return dirs;
    }

    // ============================================================================
    
    Vector2 base_fov;

    public override void OnBaseAwake()
    {
        base_fov = fov;
    }

    public override void OnSetDefault()
    {
        fov = base_fov;
    }

    // ============================================================================
        
    public override void OnBaseDrawGizmos(Vector3 start, Vector3 end)
    {
        foreach(var dir in GetConeDirs(GetRayDir()))
        {
            Vector3 cone_end = start + dir * range;
            Gizmos.DrawLine(start, cone_end);
        }
    }
}
