using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbilityCaster))]

public class HealAbility : MonoBehaviour
{
    AbilityCaster caster;

    public GameObject owner;
    public AbilitySO healSO;
    public HPManager hp;

    void Awake()
    {
        caster = GetComponent<AbilityCaster>();
    }

    // ============================================================================
    
    void Update()
    {
        UpdateBuffer();
        TryStartCasting();
    }
    
    // ============================================================================

    [Header("Before Casting")]
    public float bufferTime=.2f;
    float bufferLeft;

    public void DoBuffer() => bufferLeft = bufferTime;

    void UpdateBuffer()
    {
        bufferLeft -= Time.deltaTime;

        if(bufferLeft<0) bufferLeft=0;
    }

    bool HasBuffer() => bufferLeft>0;

    void ResetBuffer() => bufferLeft=0;

    // ============================================================================
    
    void TryStartCasting()
    {
        if(IsCasting()) return;

        if(!HasBuffer()) return;

        StartCasting();
    }

    void StartCasting()
    {
        ResetBuffer();

        caster.abilitySO = healSO;

        caster.DoBuffer();
    }
    
    // ============================================================================

    bool IsCasting()
    {
        return caster.IsCasting();
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.AbilityEvent += OnAbility;
    }
    void OnDisable()
    {
        EventM.AbilityEvent -= OnAbility;
    }

    // ============================================================================

    void OnAbility(GameObject caster, AbilitySO abilitySO)
    {
        if(caster!=owner) return;

        if(abilitySO!=healSO) return;

        hp.Add(abilitySO.magnitude);

        TempVFX(abilitySO);
    }

    // Move to vfx manager later ============================================================================

    void TempVFX(AbilitySO abilitySO)
    {
        // flash green
        SpriteManager.Current.FlashColor(owner, -1, 1, -1);
        ModelManager.Current.FlashColor(owner, -1, 1, -1);

        Vector3 top = ColliderManager.Current.GetTop(owner);

        // pop up text
        VFXManager.Current.SpawnPopUpText(top, $"+{abilitySO.magnitude}", Color.green);

        //DisableCastTrails();

        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal1, transform.position);
        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal2, transform.position);
    }
}
