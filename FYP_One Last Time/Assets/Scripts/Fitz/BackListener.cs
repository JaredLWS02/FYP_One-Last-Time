using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackListener : MonoBehaviour
{
    public UnityEvent OnBack;

    void Update()
    {
        if(ScenesManager.Current.isTransitioning) return;
        
        if(InputManager.Current.backKeyDown)
        {
            OnBack?.Invoke();
        }
    }
}