using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PrefabSpawner : MonoBehaviour
{    
    public List<PrefabPreset> prefabs;

    PrefabPreset currentPrefab;

    public void GetPrefab(string prefab_name)
    {
        if(string.IsNullOrEmpty(prefab_name))
        {
            Debug.LogWarning("Prefab name is null or empty.");
            currentPrefab = null;
            return;
        }
        
        currentPrefab = prefabs.Find(item => item.prefabName == prefab_name);
        
        if(currentPrefab==null) Debug.LogWarning($"Prefab with name '{prefab_name}' not found.");
    }

    // ============================================================================

    public void SetSpawnPos(Vector3 pos) => currentPrefab.spawnPos = pos;
    public void SetSpawnRot(Quaternion rot) => currentPrefab.spawnRot = rot;
    public void SetSpawnScaleMult(float mult) => currentPrefab.scaleMult = mult;

    // ============================================================================

    public GameObject Spawn(PrefabPreset prefab) => prefab?.Spawn();
    
    public void Spawn() => Spawn(currentPrefab);

    GameObject SpawnAndReturn(string prefab_name)
    {
        GetPrefab(prefab_name);
        return Spawn(currentPrefab);
    }

    public void SpawnName(string prefab_name) => SpawnAndReturn(prefab_name);

    // ============================================================================

    public void Despawn(PrefabPreset prefab) => prefab?.Despawn();

    public void Despawn() => Despawn(currentPrefab);
    
    public void Despawn(string prefab_name)
    {
        GetPrefab(prefab_name);
        Despawn();
    }

    // ============================================================================

    [System.Serializable]
    public struct SpawnEvents
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
    }
    [Space]
    public SpawnEvents spawnEvents;

    void OnEnable() => spawnEvents.OnEnable?.Invoke();
    void OnDisable() => spawnEvents.OnDisable?.Invoke();
}
