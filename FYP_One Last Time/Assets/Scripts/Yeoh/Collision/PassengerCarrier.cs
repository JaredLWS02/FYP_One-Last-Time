using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class PassengerCarrier : MonoBehaviour
{    
    public class Passenger
    {
        public GameObject gameObject;
        public Rigidbody rb;
        public Collider coll;
    }

    List<Passenger> passengers = new();

    // ============================================================================

    public void TryAddPassenger(Collider other)
    {
        if(other.isTrigger) return;
        Rigidbody rb = other.attachedRigidbody;
        if(!rb) return;

        Passenger new_passenger = new();

        new_passenger.rb = rb;
        new_passenger.gameObject = rb.gameObject;
        new_passenger.coll = other;

        passengers.Add(new_passenger);
    }

    public void TryAddPassenger(Collision other)
    {
        TryAddPassenger(other.collider);
    }

    // ============================================================================
    
    public void TryRemovePassenger(Collider coll)
    {
        if(coll.isTrigger) return;
        Rigidbody rb = coll.attachedRigidbody;
        if(!rb) return;

        // reversed forloop
        for(int i=passengers.Count-1; i>=0; i--)
        {
            if(passengers[i].gameObject == rb.gameObject)
            {
                passengers.RemoveAt(i);
            }
        }
    }

    public void TryRemovePassenger(Collision other)
    {
        TryRemovePassenger(other.collider);
    }

    // ============================================================================

    void OnCollisionEnter(Collision other)
    {
        TryAddPassenger(other);
    }

    void OnCollisionExit(Collision other)
    {
        TryRemovePassenger(other);
    }

    // ============================================================================
    
    public bool IsPassenger(GameObject target, out Passenger found_passenger)
    {
        foreach(var passenger in passengers)
        {
            if(target == passenger.gameObject)
            {
                found_passenger = passenger;
                return true;
            }
        }
        found_passenger=null;
        return false;
    }

    public bool IsPassenger(GameObject target)
    {
        return IsPassenger(target, out var found_passenger);
    }
}
