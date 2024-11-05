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

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.ActionCompleteEvent += OnActionComplete;

        InvokeRepeating(nameof(SlowUpdate), 0, .5f);
    }
    void OnDisable()
    {
        EventM.ActionCompleteEvent -= OnActionComplete;
    }

    // ============================================================================

    void OnActionComplete(GameObject who)
    {
        if(who!=owner) return;
        NextBehaviour();
    }

    // ============================================================================

    public HPManager hpM;

    void SlowUpdate()
    {
        CheckPhase();
    }

    void CheckPhase()
    {
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

    void NextPhase()
    {
        if(IsPerforming()) return;

        if(IsLastPhase()) return;

        CurrentLoop().Reset();

        index++;

        Perform(CurrentPhaseAnim());

        EventM.OnPhaseChanged(owner, CurrentPhase().phaseName);
    }
    
    void PrevPhase()
    {
        if(IsPerforming()) return;

        if(IsFirstPhase()) return;

        CurrentLoop().Reset();

        index--;

        Perform(CurrentPhaseAnim());

        EventM.OnPhaseChanged(owner, CurrentPhase().phaseName);
    }
}
