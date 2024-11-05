using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PakYa : MonoBehaviour
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        PlayerManager.Current.Register(gameObject);
    }
    void OnDisable()
    {
        PlayerManager.Current.Unregister(gameObject);
    }

    // ============================================================================

    void Start()
    {
        EventM.OnSpawned(gameObject);
    }
    
}
