using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackListener : MonoBehaviour
{
    public UnityEvent OnBack;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Current.backKeyDown)
        {
            OnBack?.Invoke();
        }
    }
}