using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabSpawn
{
    public GameObject prefab;
    public Transform spawnpoint;
    public bool parented=true;
    public bool followRotation=true;
}
