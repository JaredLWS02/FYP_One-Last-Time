using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootInfo
{
    public ItemSO item;
    public int quantity;
    public Vector3 contactPoint;
}

// ============================================================================

public class Loot : MonoBehaviour
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        Invoke(nameof(EnableLoot), lootDelay);
    }

    // ============================================================================

    public ItemSO item;
    public int quantity=1;

    public float lootDelay=1;
    bool canLoot;

    void EnableLoot() => canLoot=true;

    // Touch Trigger ============================================================================

    void OnCollisionStay(Collision other)
    {
        OnStay(other.collider);
    }

    void OnTriggerStay(Collider other)
    {
        OnStay(other);
    }

    // ============================================================================

    [HideInInspector]
    public Vector3 contactPoint;

    void OnStay(Collider other)
    {
        if(!canLoot) return;
        if(other.isTrigger) return;
        if(!other.attachedRigidbody) return;

        contactPoint = other.ClosestPoint(transform.position);

        PickedUp(other.attachedRigidbody.gameObject);
    }

    // ============================================================================

    public bool destroyOnLoot=true;
    bool picked;

    void PickedUp(GameObject looter)
    {
        if(picked) return;
        picked = true;

        events.OnLoot?.Invoke(contactPoint);

        EventM.OnLoot(looter, gameObject, CopyLootInfo());

        if(destroyOnLoot) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    // ============================================================================

    LootInfo CopyLootInfo()
    {
        LootInfo info = new()
        {
            item = item,
            quantity = quantity,
            contactPoint = contactPoint
        };
        return info;
    }

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent<Vector3> OnLoot;
    }
    [Space]
    public Events events;
}
