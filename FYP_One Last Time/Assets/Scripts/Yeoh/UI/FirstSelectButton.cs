using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FirstSelectButton : MonoBehaviour
{
    public void SelectButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    // ============================================================================

    void OnEnable() => selectEvents.OnEnable?.Invoke();
    void OnDisable() => selectEvents.OnDisable?.Invoke();

    [System.Serializable]
    public struct SelectEvents
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
    }
    [Space]
    public SelectEvents selectEvents;
}
