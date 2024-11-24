using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableProp : MonoBehaviour
{
    [SerializeField]
    private int health = 3;

    [SerializeField]
    private UnityEvent Hit;

    [SerializeField]
    private LayerMask playerHurtboxLayer;

    private void OnTriggerEnter(Collider other)
    {
        bool playerHit = ((1 << other.gameObject.layer) & playerHurtboxLayer) != 0;

        if (playerHit)
        {
            health--;

            if (health <= 0)
            {
                Hit?.Invoke();
            }
        }
    }
}
