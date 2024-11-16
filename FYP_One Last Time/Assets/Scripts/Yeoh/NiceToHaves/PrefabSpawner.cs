using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{    
    public PrefabPreset preset;

    public void Spawn() => preset?.Spawn();
}
