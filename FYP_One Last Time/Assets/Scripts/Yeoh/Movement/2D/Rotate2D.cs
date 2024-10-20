using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2D : MonoBehaviour
{
    public float turnSpeed=10;
    [HideInInspector]
    public float baseTurnSpeed;

    void Awake()
    {
        baseTurnSpeed = turnSpeed;
    }

    // ============================================================================

    public Vector3Int turnAxis = new(0, 0, 1);
    public Vector3 angleOffsets = new(0, 0, -90);
    public bool linearTurn;

    // ============================================================================

    public void UpdateTurn(Vector3 dir)
    {
        if(dir==Vector3.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion lookRotation = Quaternion.Euler(
            turnAxis.x>0 ? angle + angleOffsets.x : 0,
            turnAxis.y>0 ? angle + angleOffsets.y : 0,
            turnAxis.z>0 ? angle + angleOffsets.z : 0);

        transform.rotation = linearTurn ?
            Quaternion.Lerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime) : // linearly face the direction
            Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime); // smoothly face the direction
    }
}
