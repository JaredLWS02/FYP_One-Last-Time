using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointReset : MonoBehaviour
{
    public void OnEnable()
    {
        CheckpointManager.Current.ResetCheckpoint();
    }
}
