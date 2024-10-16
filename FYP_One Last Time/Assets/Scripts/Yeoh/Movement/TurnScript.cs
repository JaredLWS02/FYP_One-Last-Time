using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnScript : MonoBehaviour
{
    void Awake()
    {
        baseTurnSpeed = turnSpeed;
    }

    // ============================================================================

    public float turnSpeed=10;
    [HideInInspector]
    public float baseTurnSpeed;

    public Vector3Int turnAxis = new(0, 1, 0);

    public bool linearTurn;

    // ============================================================================

    public void UpdateTurn(Vector3 dir)
    {
        if(dir==Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(dir);

        lookRotation = Quaternion.Euler(
            turnAxis.x>0 ? lookRotation.eulerAngles.x : 0,
            turnAxis.y>0 ? lookRotation.eulerAngles.y : 0,
            turnAxis.z>0 ? lookRotation.eulerAngles.z : 0);

        transform.rotation = linearTurn ?
            Quaternion.Lerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime): // linearly face the direction
            Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime); // smoothly face the direction
    }
}
