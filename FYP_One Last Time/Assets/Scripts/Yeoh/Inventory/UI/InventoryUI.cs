using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InventoryUI : MonoBehaviour
{
    public InventorySO inventory;    

    // ============================================================================

    void Update()
    {
        if(inventory==null) return;
        
        int i=0;

        for(; i < ui_slots.Count && i < inventory.slots.Count; i++)
        {
            ui_slots[i].slot = inventory.slots[i];
        }

        // if got remaining empty ui slots, make them null
        for(; i < ui_slots.Count; i++)
        {
            ui_slots[i].slot = null;
        }
    }

    // ============================================================================

    public int maxUiSlots=45;

    public InventoryUISlot inventoryUiSlotPrefab;

    public List<InventoryUISlot> ui_slots = new();

    [ContextMenu("Spawn and Assign UI Slots")]
    void SpawnAndAssignSlots()
    {
        for(int i=0; i < maxUiSlots; i++)
        {
            InventoryUISlot newSlot = Instantiate(inventoryUiSlotPrefab, transform);
            
            #if UNITY_EDITOR
            // record to be able to undo (ctrl z)
            Undo.RegisterCreatedObjectUndo(newSlot.gameObject, "Spawn Inventory UI Slot");
            #endif
        }
        AssignSlots();
    }
    
    [ContextMenu("Assign UI Slots")]
    void AssignSlots()
    {
        ui_slots.Clear();

        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out InventoryUISlot ui_slot))
            {
                ui_slots.Add(ui_slot);
            }
        }
    }
}
