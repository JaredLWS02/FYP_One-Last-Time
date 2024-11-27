using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlatformEffector3D))]
[RequireComponent(typeof(PassengerCarrier))]

public class OneWayPlatform3D : MonoBehaviour
{
    Collider coll;
    PlatformEffector3D effector;
    PassengerCarrier carrier;

    void Awake()
    {
        coll = GetComponent<Collider>();
        effector = GetComponent<PlatformEffector3D>();
        carrier = GetComponent<PassengerCarrier>();
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.MoveEvent += OnMove;
    }
    void OnDisable()
    {
        EventM.MoveEvent -= OnMove;
    }

    // ============================================================================

    void OnMove(GameObject mover, Vector2 input)
    {
        // must press down
        if(input.y > -0.7f) return;
        // ignore if mover is not any passenger
        if(!carrier.IsPassenger(mover, out var passenger)) return;

        Collider target_coll = passenger.coll;

        TryStopCoroutine(target_coll);

        ignoringColl_crts[target_coll] = StartCoroutine(IgnoringColl(target_coll, passenger.gameObject));
    }

    // ============================================================================

    Dictionary<Collider, Coroutine> ignoringColl_crts = new();

    void TryStopCoroutine(Collider target_coll)
    {
        if(ignoringColl_crts.TryGetValue(target_coll, out var crt))
        {
            if(crt!=null) StopCoroutine(crt);
        }
    }

    public float deplatformSeconds=.5f;

    IEnumerator IgnoringColl(Collider target_coll, GameObject deplatformer)
    {
        if(target_coll)
        {
            effector.TryAddColliderToIgnore(target_coll);
            
            IgnoreColl(target_coll, true);

            EventM.OnDeplatform(deplatformer, coll, true);
        }
        
        yield return new WaitForSeconds(deplatformSeconds);

        if(target_coll)
        {
            IgnoreColl(target_coll, false);

            effector.TryRemoveColliderToIgnore(target_coll);

            EventM.OnDeplatform(deplatformer, coll, false);
        }
    }

    void IgnoreColl(Collider target_coll, bool toggle)
    {
        Physics.IgnoreCollision(target_coll, coll, toggle);
    }
}
