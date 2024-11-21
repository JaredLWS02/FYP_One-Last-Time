using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionToggler : MonoBehaviour
{
    public List<Collider> myColliders = new();
    public List<string> layerNamesToToggle = new();

    // ============================================================================

    public void ToggleIgnoreLayers(bool toggle)
    {
        Collider[] all_colliders = FindObjectsOfType<Collider>();

        foreach(var layer_name in layerNamesToToggle)
        {
            int layer = LayerMask.NameToLayer(layer_name);

            if(layer==-1)
            {
                Debug.LogWarning($"CollisionToggler: Layer '{layer_name}' not found. Skipping.");
                continue;
            }

            foreach(var other in all_colliders)
            {
                if(other.gameObject.layer != layer) continue; // skip if not match

                foreach(var my_coll in myColliders)
                {
                    Physics.IgnoreCollision(my_coll, other, toggle);
                }
            }
        }
    }
}
