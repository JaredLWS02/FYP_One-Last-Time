using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseLockToggler : MonoBehaviour
{
    MouseManager MouseM;

    void OnEnable()
    {
        MouseM = MouseManager.Current;

        events.OnEnable?.Invoke();
    }

    void OnDisable() => events.OnDisable?.Invoke();

    // ============================================================================

    public void ToggleMouseLock(bool toggle) => MouseM.LockMouse(toggle);

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
