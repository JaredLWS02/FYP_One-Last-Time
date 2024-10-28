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

        EventM.CancelCastEvent += OnCancelCast;

        abilityList.ResetCooldowns();
    }
    void OnDisable()
    {
        EventM.CancelCastEvent -= OnCancelCast;

        abilityList.ResetCooldowns();
    }

    // ============================================================================

    void Update()
    {
        UpdateProgress();

        abilityList.UpdateCooldowns();
        abilityList.CleanUp();
    }

    // ============================================================================

    [Header("On Casting")]
    public AbilitySO abilitySO;

    AbilitySlot currentSlot;

    public void TryStartCasting()
    {
        if(isCasting) return;

        if(!abilityList.HasAbility(abilitySO, out var slot)) return;

        if(slot.IsCooling()) return;

        if(!abilitySO.HasEnoughMP(MP_Manager.hp)) return;

        currentSlot = slot;

        StartCasting();
    }

    // During Casting ============================================================================

    void StartCasting()
    {
        ResetProgress();
        isCasting=true;

        EventM.OnCasting(owner, abilitySO);

        PlayCastingAnim();

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

    public void CastWindUp()
    {
        isCast=true;
    }

    public void CastRelease()
    {
        EventM.OnCastReleased(owner, abilitySO);
    }

    public void CastRecover()
    {
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

        if(!IsCasting()) return;

        StopCasting();

        CastRecover();

        EventM.OnCastCancelled(owner);
        
        PlayCancelAnim();
    }

    [Header("Cancel")]
    public string cancelAnimName = "Cancel Cast";

    void PlayCancelAnim()
    {
        EventM.OnPlayAnim(owner, cancelAnimName, abilitySO.castingAnimLayer, abilitySO.castingAnimBlendTime);
    }
}
