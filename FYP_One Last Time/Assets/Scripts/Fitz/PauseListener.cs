using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseListener : MonoBehaviour
{
    public UnityEvent<bool> onPause;
    bool isPaused;

    void Update()
    {
        if(InputManager.Current.pauseKeyDown)
        {
            if(ScenesManager.Current.isTransitioning) return;

            isPaused = !isPaused;

            onPause?.Invoke(isPaused);
        }
    }
}
