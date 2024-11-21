using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLayerCollisionToggler : MonoBehaviour
{
    public List<string> layerNames1 = new();
    public List<string> layerNames2 = new();

    // ============================================================================

    public void ToggleIgnoreLayers(bool toggle)
    {
        foreach(var layer_name1 in layerNames1)
        {
            int layer1 = LayerMask.NameToLayer(layer_name1);
            if(layer1==-1) continue; // skip if not found

            foreach(var layer_name2 in layerNames2)
            {
                int layer2 = LayerMask.NameToLayer(layer_name2);
                if(layer2==-1) continue; // skip if not found
                
                Physics.IgnoreLayerCollision(layer1, layer2, toggle);
            }
        }
    }
}
