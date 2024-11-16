using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsForMonoBehaviour : MonoBehaviour
{
    void Awake() => uEvents.Awake?.Invoke();
    void OnEnable() => uEvents.OnEnable?.Invoke();
    void OnDisable() => uEvents.OnDisable?.Invoke();
    void Start() => uEvents.Start?.Invoke();
    void Update() => uEvents.Update?.Invoke();
    void FixedUpdate() => uEvents.FixedUpdate?.Invoke();
    void LateUpdate() => uEvents.LateUpdate?.Invoke();
    void Destroy() => uEvents.Destroy?.Invoke();

    // ============================================================================

    [System.Serializable]
    public struct UEvents
    {
        public UnityEvent Awake;
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
        public UnityEvent Start;
        public UnityEvent Update;
        public UnityEvent FixedUpdate;
        public UnityEvent LateUpdate;
        public UnityEvent Destroy;
    }
    [Header("Unity Events")]
    public UEvents uEvents;
}
