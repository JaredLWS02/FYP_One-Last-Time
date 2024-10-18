using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName="SO/Inventory/ItemSO")]

public class ItemSO : ScriptableObject
{
    public AnimatorOverrideController iconAnimOV;

    public string Name;

    [TextArea]
    public string description;

    //public int maxStackSize=64;
    
    [HideInInspector]
    public int ID => GetInstanceID();
}
