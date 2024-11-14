using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityCaster : BaseAction
{   
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.CancelCastEvent += OnCancelCast;

        abilityList.CancelCooldowns();
    }
    void OnDisable()
    {
        EventM.CancelCastEvent -= OnCancelCast;

        abilityList.CancelCooldowns();
    }

    // ============================================================================

    void Update()
    {
        UpdateProgress();

        abilityList.UpdateCooldowns();
        abilityList.CleanUp();
    }

    // ============================================================================

    public HPManager mpM;
    public AbilityListSO abilityList;
    AbilitySlot currentSlot;

    public AbilitySO abilitySO;

    public void TryStartCasting()
    {
        if(IsPerforming()) return;

        if(!abilityList.HasAbility(abilitySO, out var slot)) return;

        if(!abilitySO.CanAfford(mpM.hp)) return;

        if(slot.IsCooling()) return;

        currentSlot = slot;

        StartCasting();
    }

    //AudioSource sfxCastingLoop;

    void StartCasting()
    {
        ResetProgress();

        Perform(abilitySO.castingAnim);
        Anim1_WindUp();

        EventM.OnCasting(owner, abilitySO);

        castingEvents.CastingStart?.Invoke($"{abilitySO.Name} Casting Start");
        //sfxCastingLoop = AudioManager.Current.LoopSFX(owner, SFXManager.Current.sfxCastingLoop);
    }

    // Progress ============================================================================

    public float progress {get; private set;}
    
    void ResetProgress()
    {
        progress=0;

        EventM.OnUIBarUpdate(gameObject, progress, abilitySO.castingTime);
    }

    void UpdateProgress()
    {
        if(!IsPerforming()) return;

        progress += Time.deltaTime;

        progress = Mathf.Clamp(progress, 0, abilitySO.castingTime);

        EventM.OnUIBarUpdate(gameObject, progress, abilitySO.castingTime);

        if(IsProgressDone())
        {
            StopCasting();
            
            EventM.OnCast(owner, currentSlot);
        }
    }

    bool IsProgressDone() => progress >= abilitySO.castingTime;

    // ============================================================================
    
    void StopCasting()
    {
        ResetProgress();

        CancelAnim();

        castingEvents.CastingStop?.Invoke($"{abilitySO.Name} Casting Stop");
        //if(sfxCastingLoop) AudioManager.Current.StopLoop(sfxCastingLoop);
    }
    
    // Cancel ============================================================================
    
    void OnCancelCast(GameObject who)
    {
        if(who!=owner) return;

        if(!IsPerforming()) return;

        StopCasting();
        
        EventM.OnCastCancelled(owner);
    }

    // ============================================================================

    [System.Serializable]
    public struct CastingEvents
    {
        public UnityEvent<string> CastingStart;
        public UnityEvent<string> CastingStop;
    }
    [Space]
    public CastingEvents castingEvents;
}
