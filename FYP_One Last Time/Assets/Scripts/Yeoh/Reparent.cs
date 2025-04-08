using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reparent : MonoBehaviour
{
    public void SetParent(Transform to) => transform.parent = to;

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
    }
    [Space]
    public Events events;

    // ============================================================================

    void OnEnable()
    {
        events.OnEnable?.Invoke();
    }
    void OnDisable()
    {
        events.OnDisable?.Invoke();
    }
}
