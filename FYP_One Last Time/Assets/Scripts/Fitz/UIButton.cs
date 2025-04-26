using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public void OnHover()
    {
        buttonEvents.onHoverEvent?.Invoke();
        //buttonComp.Select();
    }

    public void OffHover()
    {
        buttonEvents.offHoverEvent?.Invoke();
    }

    public void OnQuit()
    {
        ScenesManager.Current.Quit();
    }

    public void PlayGame()
    {
        ScenesManager.Current.LoadNextScene();
    }

    public void GoMainMenu()
    {
        ScenesManager.Current.LoadMainMenu();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnHover();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OffHover();
    }

    void OnDisable()
    {
        OffHover();
    }

    // ============================================================================

    [System.Serializable]
    public struct ButtonEvents
    {
        public UnityEvent onHoverEvent;
        public UnityEvent offHoverEvent;
    }
    [Space]
    public ButtonEvents buttonEvents;
}
