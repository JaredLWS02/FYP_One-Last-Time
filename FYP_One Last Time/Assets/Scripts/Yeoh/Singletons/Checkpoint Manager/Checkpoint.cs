using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform owner;

    public void SetCheckpoint()
    {
        CheckpointManager.Current.SetCheckpoint(owner.position);
    }
}
