using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anaya : MonoBehaviour
{
    public GameObject anaya;
    public GameObject player;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.CancelCastEvent += OnCancelCast;
    }
    void OnDisable()
    {
        EventM.CancelCastEvent -= OnCancelCast;
    }

    // ============================================================================

    void OnCancelCast(GameObject who)
    {
        if(who!=player) return;
        // also cancel for anaya
        EventM.OnCancelCast(anaya);
    }
}
