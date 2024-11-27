using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static JumpScript;

public class VinePullScript : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        //EventM.VinePullEvent += OnVinePull;
        //EventM.VinePullCutEvent += OnVinePullCut;
    }
    void OnDisable()
    {
        //EventM.VinePullEvent -= OnVinePull;
        //EventM.VinePullCutEvent -= OnVinePullCut;
    }

    // ============================================================================

    [Header("Vine Pull")]
    public float swingForce = 50, grabForceMult = 3;
    //public float climbUpTime=.5f, climbDownTime=.2f;
    public float climbUpSpeed = 5, climbDownSpeed = 10;
    public AnimSO vinepullAnim;

    void OnVinePull(GameObject who)
    {
        if (who != owner) return;

        
    }

    void OnVinePullCut(GameObject who)
    {

    }
}
