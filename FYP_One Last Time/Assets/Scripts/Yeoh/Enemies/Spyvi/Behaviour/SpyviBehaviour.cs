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
    public string laserKeyword="Laser";

    // ============================================================================

    [Header("Toggle Triggers")]
    public GameObject rushTrigger;
    public GameObject laserTrigger;

    public void ToggleRushTrigger(bool toggle)
    {
        rushTrigger.SetActive(toggle);
        laserTrigger.SetActive(!toggle);
    }

    public void ToggleLaserTrigger(bool toggle)
    {
        rushTrigger.SetActive(!toggle);
        laserTrigger.SetActive(toggle);
    }

}
