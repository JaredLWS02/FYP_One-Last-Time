using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLayerToggler : MonoBehaviour
{
    public List<Collider> colliders = new();

    public List<string> toggledLayerNames = new();

    public void ToggleIgnoreLayers(bool toggle)
    {
        foreach(var coll in colliders)
        {
            foreach(var layer_name in toggledLayerNames)
            {
                int layer = LayerMask.NameToLayer(layer_name);
                // skip if layer not found
                if(layer==-1) continue;

                IgnoreLayer(coll, layer, toggle);
            }
        }
    }

    void IgnoreLayer(Collider coll, int layer, bool toggle)
    {
        Physics.IgnoreLayerCollision(coll.gameObject.layer, layer, toggle);
    }
}
