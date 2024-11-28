using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseListener : MonoBehaviour
{
    public UnityEvent<bool> onPause;
    bool isPaused;

    void Start()
    {
        MouseManager.Current.LockMouse(!isPaused);
    }

    void Update()
    {
        if(InputManager.Current.pauseKeyDown)
        {
            if (isPaused) isPaused = false;
            else isPaused = true;
            
            onPause.Invoke(isPaused);
            MouseManager.Current.LockMouse(!isPaused);
        }
    }
}
