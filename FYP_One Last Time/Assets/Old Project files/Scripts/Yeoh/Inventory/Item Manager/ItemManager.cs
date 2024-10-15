using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Current;

    void Awake()
    {
        Current=this;
    }

    // ============================================================================

    public GameObject lootPrefab;

    public GameObject Spawn(Vector3 pos, ItemSO item, int quantity=1)
    {
        GameObject spawned = Instantiate(lootPrefab, pos, Quaternion.identity);

        if(spawned.TryGetComponent(out Loot2D loot))
        {
            loot.item = item;
            loot.quantity = quantity;
        }

        return spawned;
    }

    // ============================================================================
}
