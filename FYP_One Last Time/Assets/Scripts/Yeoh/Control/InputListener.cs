using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Pilot))]

public class InputListener : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;

    void Awake()
    {
        pilot = GetComponent<Pilot>();
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryInputMoveEvent += OnTryInputMove;
        EventM.TryInputJumpEvent += OnTryInputJump;
        EventM.TryInputAttackEvent += OnTryInputAttack;
        EventM.TryInputCastEvent += OnTryInputCast;
    }
    void OnDisable()
    {
        EventM.TryInputMoveEvent -= OnTryInputMove;
        EventM.TryInputJumpEvent -= OnTryInputJump;
        EventM.TryInputAttackEvent -= OnTryInputAttack;
        EventM.TryInputCastEvent -= OnTryInputCast;
    }

    // Move ============================================================================

    Vector2 moveInput;

    void OnInputMove(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        Vector2 input = value.Get<Vector2>();

        EventM.OnTryInputMove(gameObject, input);
    }

    void OnTryInputMove(GameObject mover, Vector2 input_dir)
    {
        if(mover!=gameObject) return;

        if(pilot.IsNone()) return;

        moveInput = input_dir;
    } 

    void Update()
    {
        if(pilot.IsNone()) moveInput = Vector2.zero;

        EventM.OnTryMove(gameObject, moveInput);
    }

    // Jump ============================================================================

    void OnInputJump(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        float input = value.Get<float>();

        EventM.OnTryInputJump(gameObject, input);
    }

    void OnTryInputJump(GameObject jumper, float input)
    {
        if(jumper!=gameObject) return;

        if(pilot.IsNone()) return;

        EventM.OnTryJump(gameObject, input);
    }

    // Attack ============================================================================

    void OnInputLightAttack()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnTryInputAttack(gameObject, "Light");
    }

    void OnInputHeavyAttack()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnTryInputAttack(gameObject, "Heavy");
    }

    void OnTryInputAttack(GameObject attacker, string type)
    {
        if(attacker!=gameObject) return;

        if(pilot.IsNone()) return;

        EventM.OnTryAttack(gameObject, type);
    }

    // Cast ============================================================================

    void OnInputHeal()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnTryInputCast(gameObject, "Heal");
    }

    void OnTryInputCast(GameObject caster, string type)
    {
        if(caster!=gameObject) return;

        if(pilot.IsNone()) return;

        EventM.OnTryStartCast(gameObject, type);
    }
}
