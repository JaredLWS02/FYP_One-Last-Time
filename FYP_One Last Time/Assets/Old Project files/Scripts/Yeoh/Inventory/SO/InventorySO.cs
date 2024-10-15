using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemSO item;
    public int quantity;

    public bool IsEmpty()
    {
        return item==null || quantity<=0;
    }
};

// ============================================================================

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    public List<InventorySlot> slots;

    // Getters ============================================================================

    public InventorySlot GetSlot(ItemSO item)
    {
        foreach(var slot in slots)
        {
            if(slot.item == item)
            {
                return slot;
            }
        }
        return null;
    }

    public bool HasSlot(ItemSO item)
    {
        return GetSlot(item) != null;
    }

    // Setters ============================================================================

    public void AddSlot(ItemSO item, int quantity)
    {
        if(HasSlot(item))
        {
            Debug.Log($"Already have slot: {item.Name}");
            return;
        }

        InventorySlot slot = new()
        {
            item = item,
            quantity = quantity
        };

        slots.Add(slot);
    }

    public void RemoveSlot(InventorySlot slot)
    {
        if(!slots.Contains(slot)) return;

        slots.Remove(slot);
    }

    public void RemoveAllSlots()
    {
        slots.Clear();
    }

    // Leftovers ============================================================================

    public void CleanUp()
    {
        slots.RemoveAll(slot => slot.IsEmpty());
    }
}

