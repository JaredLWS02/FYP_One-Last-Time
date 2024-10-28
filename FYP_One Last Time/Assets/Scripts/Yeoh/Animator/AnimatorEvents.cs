using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimPreset
{
    public List<string> names = new();
    public int layer=0;
    public float blendTime=0;
    public string cancelName = "Idle";

    public string GetRandomName()
    {
        if(names.Count<=0) return "";

        return names[Random.Range(0, names.Count)];
    }

    public void Play(GameObject who, string name)
    {
        EventManager.Current.OnPlayAnim(who, name, layer, blendTime);
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
}

// ============================================================================

public class AnimatorEvents : MonoBehaviour
{
    public GameObject owner;
    public Animator anim;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.PlayAnimEvent += OnPlayAnim;
    }
    void OnDisable()
    {
        EventM.PlayAnimEvent -= OnPlayAnim;
    }
    
    // ============================================================================
    
    void OnPlayAnim(GameObject who, string animName, int animLayer, float blendTime)
    {
        if(who!=owner) return;

        if(blendTime <= 0)
        {
            anim.Play(animName, animLayer, 0);
        }
        else
        {
            anim.CrossFade(animName, blendTime, animLayer, 0);
        }
    }
}
