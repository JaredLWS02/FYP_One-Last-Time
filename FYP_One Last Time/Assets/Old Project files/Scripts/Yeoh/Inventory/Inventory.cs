using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySO inventory;

    // ============================================================================

    void Update()
    {
        inventory.CleanUp();
    }

    // Setters ============================================================================

    public void AddItem(ItemSO item, int quantity)
    {
        if(quantity<=0) return;

        if(HasItem(item))
        {
            InventorySlot slot = inventory.GetSlot(item);

            slot.quantity += quantity;
        }
        else 
        {
            inventory.AddSlot(item, quantity);
        }
    }
    
    public void RemoveItem(ItemSO item, int quantity)
    {
        if(!HasItem(item)) return;

        InventorySlot slot = inventory.GetSlot(item);

        slot.quantity -= quantity;

        if(slot.quantity<=0)
        {
            inventory.RemoveSlot(slot);
        }
    }

    public void Clear()
    {
        inventory.RemoveAllSlots();
    }

    // Getters ============================================================================

    public bool HasItem(ItemSO item, int quantity=1)
    {
        if(!inventory.HasSlot(item)) return false;

        InventorySlot slot = inventory.GetSlot(item);

        if(slot.quantity >= quantity) return true;

        return false;
    }

    public int GetQuantity(ItemSO item)
    {
        if(!HasItem(item)) return 0;

        InventorySlot slot = inventory.GetSlot(item);

        return slot.quantity;
    }

    // Actions ============================================================================

    public void Drop(ItemSO item, int quantity, bool stacked=true)
    {
        if(!HasItem(item)) return;

        quantity = Mathf.Min(quantity, GetQuantity(item));

        if(stacked)
        {
            ItemManager.Current.Spawn(transform.position, item, quantity);
        }
        else
        {
            for(int i=0; i<quantity; i++)
            {
                ItemManager.Current.Spawn(transform.position, item);
            }
        }

        RemoveItem(item, quantity);
    }

    public void DropAll(bool stacked=true)
    {
        List<InventorySlot> slotsToRemove = new();

        foreach(var slot in inventory.slots)
        {
            slotsToRemove.Add(slot);
        }

        foreach(var slot in slotsToRemove)
        {
            Drop(slot.item, slot.quantity, stacked);
        }
    }
    
}
