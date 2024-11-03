using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Anim", menuName="SO/AnimSO")]

public class AnimSO : ScriptableObject
{
    public List<string> names = new();
    public int layer=1;
    public float blendTime=0;
    public string cancelName = "Cancel";

    // ============================================================================
    
    public string GetRandomName()
    {
        if(names.Count<=0) return "";

        return names[Random.Range(0, names.Count)];
    }

    public void Play(GameObject who, string any_name)
    {
        EventManager.Current.OnPlayAnim(who, any_name, layer, blendTime);
    }

    public void Play(GameObject who)
    {
        if(names.Count<=0) return;

        Play(who, GetRandomName());
    }

    public void Cancel(GameObject who)
    {
        Play(who, cancelName);
    }

    // ============================================================================

    [HideInInspector]
    public int ID => GetInstanceID();
}
