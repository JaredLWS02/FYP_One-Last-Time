using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyviBehaviour : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    [Header("Check Behaviour State")]
    public PhaseScript phase;
    public string CurrentBehaviour() => phase.CurrentBehaviour();

    // ============================================================================

    [Header("Keywords")]
    public string rushKeyword="Rush";

    // ============================================================================

    [Header("Toggle Triggers")]
    public GameObject rushTrigger;

    public void ToggleRushTrigger(bool toggle)
    {
        if(rushTrigger)
        rushTrigger.SetActive(toggle);
    }
}
