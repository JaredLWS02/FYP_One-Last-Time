using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseScript : BaseAction
{
    [System.Serializable]
    public class Phase
    {
        public string phaseName;
        public SequenceLoop loop;
        public float HPPercent=100;
        public AnimSO phaseAnim;
    }

    [Header("Phase Script")]
    public List<Phase> phases = new();
    int index=0;

    // ============================================================================
    
    Phase CurrentPhase() => phases[index];
    bool IsFirstPhase() => index <= 0;
    bool IsLastPhase() => index >= phases.Count-1;

    Phase GetNextPhase()
    {
        if(IsLastPhase()) return null;
        return phases[index+1];
    }
    Phase GetPrevPhase()
    {
        if(IsFirstPhase()) return null;
        return phases[index-1];
    }

    // ============================================================================
    
    SequenceLoop CurrentLoop() => CurrentPhase().loop;
    public string CurrentBehaviour() => CurrentLoop().CurrentOption();
    void NextBehaviour() => CurrentLoop().Next();
    
    // ============================================================================
    
    float GetHPPercent(Phase phase) => phase?.HPPercent ?? float.NaN;
    float CurrentHPPercent() => GetHPPercent(CurrentPhase());
    float NextHPPercent() => GetHPPercent(GetNextPhase());
    float PrevHPPercent() => GetHPPercent(GetPrevPhase());

    // ============================================================================

    AnimSO CurrentPhaseAnim() => CurrentPhase()?.phaseAnim;
    AnimSO NextPhaseAnim() => GetNextPhase().phaseAnim;
    AnimSO PrevPhaseAnim() => GetPrevPhase().phaseAnim;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.ActionCompleteEvent += OnActionComplete;
        EventM.TryChangePhaseEvent += OnTryChangePhase;
    }
    void OnDisable()
    {
        EventM.ActionCompleteEvent -= OnActionComplete;
        EventM.TryChangePhaseEvent -= OnTryChangePhase;
    }

    // ============================================================================

    void OnActionComplete(GameObject who)
    {
        if(who!=owner) return;
        NextBehaviour();
    }

    // ============================================================================

    public HPManager hpM;

    void OnTryChangePhase(GameObject who)
    {
        if(who!=owner) return;
        
        float hp = hpM.GetHPPercent();

        if(hp <= NextHPPercent())
        {
            NextPhase();
        }
        else if(hp > CurrentHPPercent())
        {
            PrevPhase();
        }
    }

    // ============================================================================
    
    int phase_direction = 1;

    void NextPhase()
    {
        if(IsLastPhase()) return;

        if(IsPerforming()) return;

        phase_direction = 1;

        Perform(NextPhaseAnim());
    }
    
    void PrevPhase()
    {
        if(IsFirstPhase()) return;

        if(IsPerforming()) return;

        phase_direction = -1;

        Perform(PrevPhaseAnim());
    }

    // ============================================================================

    public override void OnAnimRecover()
    {
        CurrentLoop().Reset();

        index += phase_direction;

        EventM.OnPhaseChanged(owner, CurrentPhase().phaseName);
    }

    // Cancel ============================================================================

    void OnCancelPhaseChange(GameObject who)
    {
        if(who!=owner) return;

        if(!IsPerforming()) return;

        CancelAnim();
    }
}
