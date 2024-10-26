using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Pilot))]

public class PlayerInputListener : MonoBehaviour
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
    }

    // Move ============================================================================

    Vector2 moveInput;

    void Update()
    {
        if(pilot.IsNone()) moveInput = Vector2.zero;

        EventM.OnTryMove(gameObject, moveInput);
        EventM.OnTryFaceX(gameObject, moveInput.x);
    }

    void OnInputMove(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        Vector2 input_dir = value.Get<Vector2>();

        moveInput = input_dir;
    }

    // Jump ============================================================================

    void OnInputJump(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        float input = value.Get<float>();

        EventM.OnTryJump(gameObject, input);
    }

    // Attack ============================================================================

    void OnInputLightAttack()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnTryAttack(gameObject, "Light Combo");
    }

    void OnInputHeavyAttack()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnTryAttack(gameObject, "Heavy Combo");
    }

    // Cast ============================================================================

    void OnInputHeal()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnTryAbility(gameObject, "Heal");
    }
}
