using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{
    [System.Serializable]
    public class ItemDrop
    {
        public ItemSO item;
        public Vector2Int quantity = new(1, 1);
        public bool stacked=false;
        public float percent=100;
    }

    // ============================================================================

    public List<ItemDrop> drops = new();

    public void Drop()
    {
        foreach(ItemDrop drop in drops)
        {
            if(Random.Range(0, 100f) <= drop.percent)
            {
                int quantity = Random.Range(drop.quantity.x, drop.quantity.y+1);

                if(drop.stacked)
                {
                    ItemManager.Current.Spawn(transform.position, drop.item, quantity);
                }
                else
                {
                    for(int i=0; i<quantity; i++)
                    {
                        ItemManager.Current.Spawn(transform.position, drop.item);
                    }
                }
            }
        }
    }
}
