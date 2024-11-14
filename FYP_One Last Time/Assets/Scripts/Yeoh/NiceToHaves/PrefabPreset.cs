using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabPreset
{
    public GameObject prefab;
    public bool hideInHierarchy;
    [Space]
    public Transform spawnpoint;
    public bool parented=true;
    public bool followRotation=true;

    // optional
    [HideInInspector]
    public Vector3 spawnPos;
    [HideInInspector]
    public Quaternion spawnRot = Quaternion.identity;

    // ============================================================================

    public GameObject Spawn()
    {
        Vector3 pos;
        Quaternion rot;
        Transform parent=null;
        
        if(spawnpoint)
        {
            pos = spawnpoint.position;
            rot = followRotation ? spawnpoint.rotation : Quaternion.identity;
            if(parented) parent = spawnpoint;
        }
        else
        {
            pos = spawnPos;
            rot = spawnRot;
            parent = null;
        }

        return Singleton.Current.Spawn(prefab, pos, rot, parent, hideInHierarchy);
    }

    public void Despawn(GameObject obj, float delay=0) => Singleton.Current.Despawn(obj, delay);
}
