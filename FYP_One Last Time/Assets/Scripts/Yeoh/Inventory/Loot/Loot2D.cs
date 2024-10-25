using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInfo
{
    public ItemSO item;
    public int quantity;
    public Vector3 contactPoint;
}

// ============================================================================

public class Loot2D : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator anim;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        Push();

        Invoke(nameof(EnableLoot), lootDelay);
    }

    // Sprite/Anim ============================================================================

    public ItemSO item;
    public int quantity=1;

    void Update()
    {
        if(anim) anim.Play($"{item}", 0);
    }

    // Loot Delay ============================================================================

    public float lootDelay=1;
    bool canLoot;

    void EnableLoot()
    {
        canLoot=true;
    }

    // Touch Trigger ============================================================================

    [HideInInspector] public Vector3 contactPoint;

    void OnTriggerStay2D(Collider2D other)
    {
        if(!canLoot) return;
        if(other.isTrigger) return;
        if(!other.attachedRigidbody) return;

        contactPoint = other.ClosestPoint(transform.position);

        Pickup(other.attachedRigidbody.gameObject);
    }

    // ============================================================================

    public bool destroyOnLoot=true;
    bool picked;

    void Pickup(GameObject looter)
    {
        if(picked) return;
        picked=true;

        EventM.OnLoot(looter, gameObject, CopyLootInfo());

        if(destroyOnLoot) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

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

    // Physics ============================================================================

    public void Push(float force=1.5f)
    {
        Vector2 randVector = new Vector2
        (
            Random.Range(-force, force),
            Random.Range(-force, force)
        );

        rb.AddForce(randVector, ForceMode2D.Impulse);
    }
}
