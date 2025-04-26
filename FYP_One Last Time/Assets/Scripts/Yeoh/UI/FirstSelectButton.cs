using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FirstSelectButton : MonoBehaviour
{
    public void SelectButton(GameObject button)
    {
        StartCoroutine(SelectFirstButtonNextFrame(button));
    }

    IEnumerator SelectFirstButtonNextFrame(GameObject obj)
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
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
