using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableProp : MonoBehaviour
{
    [SerializeField]
    private UnityEvent Hit;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<ControllerMovement>();
        if (player)
        {
            Hit?.Invoke();
        }
    }
}
