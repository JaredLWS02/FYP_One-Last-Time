using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnayaActionInputs : MonoBehaviour
{
    public GameObject owner;
    public Pilot pilot;

    [Header("Input Buffers")]
    public InputBuffer inputBuffer;
    public float abilityBuffer=.2f;

    // ============================================================================
    
    void Update()
    {
        if(!pilot.IsPlayer()) return;

        CheckAbility1();
        CheckAbility2();
        CheckAbility3();
    }

    void CheckAbility1()
    {
        if(InputM.ability1KeyDown)
        inputBuffer.AddBuffer("Ability1", abilityBuffer);
    }

    void CheckAbility2()
    {
        if(InputM.ability2KeyDown)
        inputBuffer.AddBuffer("Ability2", abilityBuffer);
    }

    void CheckAbility3()
    {
        if(InputM.ability3KeyDown)
        inputBuffer.AddBuffer("Ability3", abilityBuffer);
    }
    
    // Input Buffer ============================================================================

    InputManager InputM;
    EventManager EventM;

    void OnEnable()
    {
        InputM = InputManager.Current;
        EventM = EventManager.Current;

        inputBuffer.InputBufferingEvent += OnInputBuffering;

        EventM.CastingEvent += OnCasting;
        EventM.CastCancelledEvent += OnCastCancelled;
    }
    void OnDisable()
    {
        inputBuffer.InputBufferingEvent -= OnInputBuffering;

        EventM.CastingEvent -= OnCasting;
        EventM.CastCancelledEvent -= OnCastCancelled;
    }
    
    // ============================================================================    

    void OnInputBuffering(string input_name)
    {
        switch(input_name)
        {
            case "Ability1": EventM.OnTryAbility(owner, "Anaya Heal"); break;

            case "Ability2": EventM.OnTryAbility(owner, "Anaya Heal"); break;

            case "Ability3": EventM.OnTryAbility(owner, "Anaya Heal"); break;
        }
    }

    // ============================================================================    

    void OnCasting(GameObject caster, AbilitySO abilitySO)
    {
        if(caster!=owner) return;

        inputBuffer.RemoveBuffer("Ability1");
        inputBuffer.RemoveBuffer("Ability2");
        inputBuffer.RemoveBuffer("Ability3");
    }

    void OnCastCancelled(GameObject caster)
    {
        if(caster!=owner) return;

        inputBuffer.RemoveBuffer("Ability1");
        inputBuffer.RemoveBuffer("Ability2");
        inputBuffer.RemoveBuffer("Ability3");
    }

}
