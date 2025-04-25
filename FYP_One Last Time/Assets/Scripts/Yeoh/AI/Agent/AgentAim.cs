using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAim : MonoBehaviour
{
    public Transform owner;
    public AgentTargeting targeting;
    public Transform aimer;

    public void Aim()
    {
        if(!targeting.target) return;

        Vector3 dir = (targeting.target.transform.position - owner.position).normalized;

        aimer.forward = dir;
    }
}
