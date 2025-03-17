using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform owner;

    void Start()
    {
        CheckpointManager.Current.GoToCheckpoint(owner);
    }
}
