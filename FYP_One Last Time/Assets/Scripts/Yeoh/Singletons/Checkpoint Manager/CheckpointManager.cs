using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    // ============================================================================

    Vector3? checkpoint = null;

    public void SetCheckpoint(Vector3 where) => checkpoint = where;

    public void ResetCheckpoint() => checkpoint = null;

    public void GoToCheckpoint(Transform who)
    {
        if(checkpoint == null) return;
        who.position = checkpoint.Value;
    }

    // ============================================================================

    [Header("Debug")]
    public Vector3 checkpointPosition = Vector3.zero;

    void Update()
    {
        checkpointPosition = checkpoint != null ? checkpoint.Value : Vector3.zero;
    }
}
