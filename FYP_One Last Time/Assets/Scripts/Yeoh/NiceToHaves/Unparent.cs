using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unparent : MonoBehaviour
{
    void OnEnable() => events.OnEnable?.Invoke();
    void OnDisable() => events.OnDisable?.Invoke();

    // ============================================================================

    public void UnparentMe() => transform.parent = null;

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
    }
    [Space]
    public Events events;
}
