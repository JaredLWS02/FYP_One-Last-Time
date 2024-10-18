using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlatformEffector3D))]

public class OneWayPlatform3D : MonoBehaviour
{
    Collider coll;
    PlatformEffector3D effector;

    void Awake()
    {
        coll = GetComponent<Collider>();
        effector = GetComponent<PlatformEffector3D>();
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.MoveYEvent += OnMoveY;
    }
    void OnDisable()
    {
        EventManager.Current.MoveYEvent -= OnMoveY;
    }

    // ============================================================================

    void OnMoveY(GameObject mover, float input_y)
    {
        if(input_y > -0.7f) return;

        foreach(var passenger in passengers)
        {
            if(mover == passenger.gameObject)
            {
                if(passenger.timer!=null) StopCoroutine(passenger.timer);
                passenger.timer = StartCoroutine(IgnoringColl(passenger.coll));
            }
        }
    }

    // ============================================================================

    void OnCollisionEnter(Collision other)
    {
        Rigidbody rb = other.rigidbody;
        if(!rb) return;

        Passenger new_passenger = NewPassenger(other);
    
        passengers.Add(new_passenger);
    }

    void OnCollisionExit(Collision other)
    {
        Rigidbody rb = other.rigidbody;
        if(!rb) return;

        // reversed forloop
        for(int i=passengers.Count-1; i>=0; i--)
        {
            if(passengers[i].rb==rb)
            {
                passengers.RemoveAt(i);
            }
        }
    }

    // ============================================================================

    class Passenger
    {
        public GameObject gameObject;
        public Rigidbody rb;
        public Collider coll;
        public Coroutine timer;
    }

    List<Passenger> passengers = new();

    Passenger NewPassenger(Collision other)
    {
        Rigidbody rb = other.rigidbody;

        Passenger new_passenger = new();

        new_passenger.gameObject = rb.gameObject;
        new_passenger.rb = rb;
        new_passenger.coll = other.collider;

        return new_passenger;
    }

    // ============================================================================

    IEnumerator IgnoringColl(Collider coll)
    {
        if(coll)
        {
            if(!effector.collidersToIgnore.Contains(coll))
            effector.collidersToIgnore.Add(coll);
            
            IgnoreColl(coll, true);
        }
        
        yield return new WaitForSeconds(.5f);

        if(coll)
        {
            IgnoreColl(coll, false);

            if(effector.collidersToIgnore.Contains(coll))
            effector.collidersToIgnore.Remove(coll);
        }
    }

    void IgnoreColl(Collider targetColl, bool toggle)
    {
        Physics.IgnoreCollision(targetColl, coll, toggle);
    }
}
