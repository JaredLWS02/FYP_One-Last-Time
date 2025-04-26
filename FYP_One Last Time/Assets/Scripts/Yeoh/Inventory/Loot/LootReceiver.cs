using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootReceiver : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.LootEvent += OnLoot;
    }
    void OnDisable()
    {
        EventM.LootEvent -= OnLoot;
    }

    // ============================================================================

    public List<ItemSO> itemWhitelist = new();

    bool HasWhitelist() => itemWhitelist.Count > 0;

    bool IsItemWhitelisted(ItemSO target)
    {
        foreach(var item in itemWhitelist)
        {
            if(item == target) return true;
        }
        return false;
    }

    // ============================================================================

    void OnLoot(GameObject looter, GameObject loot, LootInfo loot_info)
    {
        if(looter != owner) return;

        if(HasWhitelist())
        {
            if(!IsItemWhitelisted(loot_info.item)) return;

            LootEvents(loot_info);
        }
        else LootEvents(loot_info);
    }

    // ============================================================================

    void LootEvents(LootInfo loot_info)
    {
        events.OnLootItem?.Invoke(loot_info.item);
        events.OnLootQuantity?.Invoke(loot_info.quantity);
        events.OnLootQuantityF?.Invoke(loot_info.quantity);
    }

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent<ItemSO> OnLootItem;
        public UnityEvent<int> OnLootQuantity;
        public UnityEvent<float> OnLootQuantityF;
    }
    [Space]
    public Events events;
}
