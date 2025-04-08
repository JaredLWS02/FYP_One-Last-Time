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
    public string shootTyreKeyword="Shoot Tyre";

    // ============================================================================

    [Header("Toggle Triggers")]
    public GameObject rushTrigger;
    public GameObject laserTrigger;
    public GameObject shootTyreTrigger;
}
