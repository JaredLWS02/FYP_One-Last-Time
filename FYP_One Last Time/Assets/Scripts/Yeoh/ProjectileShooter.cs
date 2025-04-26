using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileShooter : MonoBehaviour
{
    public List<GameObject> prefabs = new();

    GameObject GetRandomPrefab()
    {
        if(prefabs.Count<=0) return null;
        return prefabs[Random.Range(0, prefabs.Count)];
    }

    // ============================================================================
    
    public Transform firepoint;
    public bool followRotation = true;

    public void Shoot()
    {
        if(!firepoint) return;

        var prefab = GetRandomPrefab();
        if(!prefab) return;

        events.OnPreShot?.Invoke();

        var projectile = Instantiate(prefab, firepoint.position, followRotation ? firepoint.rotation : Quaternion.identity);

        Push(projectile);

        events.OnPostShot?.Invoke();
    }

    // ============================================================================

    public void ShootMultiple(int num)
    {
        for(int i=0; i<num; i++)
        {
            Shoot();
        }
    }

    // ============================================================================

    public float force = 10;

    void Push(GameObject projectile)
    {
        if(!projectile) return;
        
        var rb = projectile.GetComponent<Rigidbody>();
        if(rb) rb.AddForce(firepoint.forward * force, ForceMode.Impulse);
    }

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnPreShot;
        public UnityEvent OnPostShot;
    }
    [Space]
    public Events events;
}
