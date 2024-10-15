using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AISidePathseeker))]

public class AnayaAI : MonoBehaviour
{
    AISidePathseeker seeker;

    void Awake()
    {
        seeker = GetComponent<AISidePathseeker>();
    }

    // ============================================================================

    public enum Action
    {
        Idle,
        Seeking,
        Fleeing,
        Staying,
    }

    public Action action = Action.Seeking;

    // ============================================================================

    public void SeekMove()
    {
        seeker.Move();
    }    
}
