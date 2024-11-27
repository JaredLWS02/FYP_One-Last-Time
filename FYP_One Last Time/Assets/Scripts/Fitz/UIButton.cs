using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    Button buttonComp;

    private void Start()
    {
        buttonComp = GetComponent<Button>();
    }

    public void OnHover()
    {
        if (buttonComp.interactable)
        {
            buttonEvents.hoverEvent?.Invoke();
            buttonComp.Select();
        }
    }

    public void OnQuit()
    {
        ScenesManager.Current.Quit();
    }

    public void PlayGame()
    {
        ScenesManager.Current.LoadNextScene();
    }

    // ============================================================================

    [System.Serializable]
    public struct ButtonEvents
    {
        public UnityEvent hoverEvent;
    }
    [Space]
    public ButtonEvents buttonEvents;
}
