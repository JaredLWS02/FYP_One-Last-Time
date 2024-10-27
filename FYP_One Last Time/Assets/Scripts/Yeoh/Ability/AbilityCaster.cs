using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{   
    public GameObject owner;

    public AbilityListSO abilityList;

    public HPManager MP_Manager;

    // ============================================================================
    
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.CastWindUpEvent += OnCastWindUp;
        EventM.CastReleaseEvent += OnCastRelease;
        EventM.CastRecoverEvent += OnCastRecover;

        EventM.CancelCastEvent += OnCancelCast;

        abilityList.ResetCooldowns();
    }
    void OnDisable()
    {
        EventM.CastWindUpEvent -= OnCastWindUp;
        EventM.CastReleaseEvent -= OnCastRelease;
        EventM.CastRecoverEvent -= OnCastRecover;

        EventM.CancelCastEvent -= OnCancelCast;

        abilityList.ResetCooldowns();
    }

    // ============================================================================

    void Update()
    {
        UpdateBuffer();
        TryStartCasting();
        UpdateProgress();

        abilityList.UpdateCooldowns();
        abilityList.CleanUp();
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

    [Header("On Casting")]
    public AbilitySO abilitySO;

    AbilitySlot currentSlot;

    void TryStartCasting()
    {
        if(isCasting) return;

        if(!HasBuffer()) return;

        if(!abilityList.HasAbility(abilitySO, out var slot)) return;

        if(slot.IsCooling()) return;

        if(!abilitySO.HasEnoughMP(MP_Manager.hp)) return;

        currentSlot = slot;

        StartCasting();
    }

    // During Casting ============================================================================

    void StartCasting()
    {
        ResetBuffer();

        ResetProgress();
        isCasting=true;

        PlayCastingAnim();

        EventM.OnCasting(owner, abilitySO);

        //sfxCastingLoop = AudioManager.Current.LoopSFX(owner, SFXManager.Current.sfxCastingLoop);
    }

    void PlayCastingAnim()
    {
        EventM.OnPlayAnim(owner, abilitySO.castingAnimName, abilitySO.castingAnimLayer, abilitySO.castingAnimBlendTime);
    }

    // Progress ============================================================================

    public bool isCasting {get; private set;}

    public float progress {get; private set;}
    
    void ResetProgress()
    {
        progress=0;
        
        EventM.OnUIBarUpdate(gameObject, progress, abilitySO.castingTime);
    }

    bool IsProgressDone() => progress >= abilitySO.castingTime;

    void UpdateProgress()
    {
        if(!isCasting) return;

        progress += Time.deltaTime;

        progress = Mathf.Clamp(progress, 0, abilitySO.castingTime);

        EventM.OnUIBarUpdate(gameObject, progress, abilitySO.castingTime);

        if(IsProgressDone())
        {
            Cast();
        }
    }

    void StopCasting()
    {
        if(!isCasting) return;

        isCasting=false;
        ResetProgress();

        //if(sfxCastingLoop) AudioManager.Current.StopLoop(sfxCastingLoop);
    }

    //AudioSource sfxCastingLoop;

    // After Cast ============================================================================

    public bool isCast {get; private set;}

    void Cast()
    {
        StopCasting();

        // mp cost
        MP_Manager.Deplete(abilitySO.mpCost);

        currentSlot.DoCooldown();

        EventM.OnCast(owner, abilitySO);

        PlayCastAnim();
    }

    void PlayCastAnim()
    {
        EventM.OnPlayAnim(owner, abilitySO.castAnimName, abilitySO.castAnimLayer, abilitySO.castAnimBlendTime);
    }

    // Cast Anim Events ============================================================================

    void OnCastWindUp(GameObject caster)
    {
        if(caster!=owner) return;

        isCast=true;
    }

    void OnCastRelease(GameObject caster)
    {
        if(caster!=owner) return;

        EventM.OnAbility(owner, abilitySO);
    }

    void OnCastRecover(GameObject caster)
    {
        if(caster!=owner) return;

        isCast=false;
    }

    // ============================================================================
    
    public bool IsCasting()
    {
        return isCasting || isCast;
    }

    // Cancel ============================================================================
    
    void OnCancelCast(GameObject who)
    {
        if(who!=owner) return;
        
        CancelAnim();
    }

    [Header("Cancel")]
    public string cancelAnimName = "Cancel Cast";

    void CancelAnim()
    {
        if(!IsCasting()) return;

        StopCasting();

        EventM.OnCastRecover(owner);

        EventM.OnPlayAnim(owner, cancelAnimName, abilitySO.castingAnimLayer, abilitySO.castingAnimBlendTime);
    }
}
