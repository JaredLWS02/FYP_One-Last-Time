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
        public float hpPercent=100;
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
    public void NextBehaviour() => CurrentLoop().Next();

    public void NextBehaviourNameCheck(string behaviour_name)
    {
        if(CurrentBehaviour() == behaviour_name)
        {
            NextBehaviour();
        }
    }
    
    // ============================================================================
    
    float GetPhaseHPPercent(Phase phase) => phase?.hpPercent ?? float.NaN;
    float CurrentPhaseHPPercent() => GetPhaseHPPercent(CurrentPhase());
    float NextPhaseHPPercent() => GetPhaseHPPercent(GetNextPhase());
    float PrevPhaseHPPercent() => GetPhaseHPPercent(GetPrevPhase());

    // ============================================================================

    AnimSO CurrentPhaseAnim() => CurrentPhase()?.phaseAnim;
    AnimSO NextPhaseAnim() => GetNextPhase().phaseAnim;
    AnimSO PrevPhaseAnim() => GetPrevPhase().phaseAnim;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    public HPManager hpM;

    public bool canReversePhase;

    public void TryChangePhase()
    {
        float hp = hpM.GetHPPercent();

        if(hp <= NextPhaseHPPercent())
        {
            NextPhase();
        }
        else if(hp > CurrentPhaseHPPercent())
        {
            PrevPhase();
        }
    }

    // ============================================================================
    
    int phaseDirection = 1;

    void NextPhase()
    {
        if(IsLastPhase()) return;
        if(IsPerforming()) return;

        phaseDirection = 1;

        Perform(NextPhaseAnim());
    }
    
    void PrevPhase()
    {
        if(!canReversePhase) return;
        if(IsFirstPhase()) return;
        if(IsPerforming()) return;

        phaseDirection = -1;

        Perform(PrevPhaseAnim());
    }

    // ============================================================================

    public override void OnAnimRecover()
    {
        CurrentLoop().Reset();

        index += phaseDirection;

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
