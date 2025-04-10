using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabPreset
{
    public string prefabName;

    [SerializeField]
    List<GameObject> randomPrefabs = new();

    public bool HasPrefabs() => randomPrefabs.Count > 0;

    public GameObject GetRandomPrefab()
    {
        if(!HasPrefabs()) return null;
        return randomPrefabs[Random.Range(0,randomPrefabs.Count)];
    }

    protected List<GameObject> spawns = new();

    public bool hideInHierarchy;
    [Space]
    public Transform spawnpoint;

    [Header("Offsets")]
    public Vector3 posOffset;
    public Vector3 angleOffset;
    public float scaleMult=1;

    [Header("Parent")]
    public bool parented;
    public bool followRotation=true;

    // optional
    [HideInInspector]
    public Vector3 spawnPos;
    [HideInInspector]
    public Quaternion spawnRot = Quaternion.identity;

    // ============================================================================

    public GameObject Spawn()
    {
        RemoveNulls();

        var prefab = GetRandomPrefab();
        if(!prefab) return null;

        Singleton singleton = Singleton.Current;

        if(!singleton)
        {
            Debug.LogError($"PrefabPreset: can't spawn {prefabName} because Singleton is null. PS: Singleton won't be available on Awake");
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

        spawns.Add(spawned);

        return spawned;
    }

    // ============================================================================

    public void Despawn(GameObject obj, float delay=0)
    {
        if(!obj) return;
        if(!spawns.Contains(obj)) return;

        spawns.Remove(obj);
        Singleton.Current.Despawn(obj, delay);
    }

    public void Despawn(float delay=0)
    {
        RemoveNulls();

        // temp duplicate list to iterate through
        // because original list is changing each time 
        List<GameObject> spawns_copy = new(spawns);

        foreach(var spawned in spawns_copy)
        {
            Despawn(spawned, delay);
        }
    }

    // ============================================================================

    void RemoveNulls() => spawns.RemoveAll(item => item == null);
}
