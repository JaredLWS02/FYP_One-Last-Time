using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBehaviourManager : MonoBehaviour
{
    public GameObject owner;
    public HPManager hpM;

    // ============================================================================

    [System.Serializable]
    public class Phase
    {
        public string phaseName;
        public SequenceLoop loop;
        public float HPPercent=100;
    }

    public List<Phase> phases = new();

    int index=0;

    // ============================================================================
    
    Phase CurrentPhase() => phases[index];

    bool IsFirstPhase() => index <= 0;
    bool IsLastPhase() => index >= phases.Count;

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
    
    float GetHPPercent(Phase phase)
    {
        if(phase==null) return float.NaN;

        return phase.HPPercent;
    }

    float CurrentHPPercent() => GetHPPercent(CurrentPhase());
    float NextHPPercent() => GetHPPercent(GetNextPhase());
    float PrevHPPercent() => GetHPPercent(GetPrevPhase());

    SequenceLoop CurrentLoop() => CurrentPhase().loop;

    public string CurrentBehaviour() => CurrentLoop().CurrentOption();
    void NextBehaviour() => CurrentLoop().Next();

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.ActionCompleteEvent += OnActionComplete;
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

    void Update()
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
        if(index >= phases.Count) return;

        CurrentLoop().Reset();

        index++;

        EventM.OnPhaseChanged(owner, CurrentPhase().phaseName);
    }
    
    void PrevPhase()
    {
        if(index<=0) return;

        CurrentLoop().Reset();

        index--;

        EventM.OnPhaseChanged(owner, CurrentPhase().phaseName);
    }
}
