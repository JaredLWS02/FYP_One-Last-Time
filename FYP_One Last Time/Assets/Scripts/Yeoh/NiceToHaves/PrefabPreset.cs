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

    [Header("Offsets")]
    public Vector3 posOffset;
    public Vector3 angleOffset;
    public float scaleMult=1;

    [Header("Parent")]
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
        if(!prefab)
        {
            Debug.LogError($"PrefabPreset: Woiii, where prefab, brother?");
            return null;
        }

        Singleton singleton = Singleton.Current;

        if(!singleton)
        {
            Debug.LogError($"PrefabPreset: can't spawn {prefab.name} because Singleton is null. PS: Singleton won't be available on Awake");
            return null;
        }

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

        pos += posOffset;
        rot.eulerAngles += angleOffset;

        GameObject spawned = singleton.Spawn(prefab, pos, rot, parent, hideInHierarchy);

        spawned.transform.localScale *= scaleMult;

        return spawned;
    }

    public void Despawn(GameObject obj, float delay=0) => Singleton.Current.Despawn(obj, delay);
}
