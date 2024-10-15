using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pilot))]
[RequireComponent(typeof(Jump2D))]
[RequireComponent(typeof(WolfAI))]

public class Wolf : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;
    Jump2D jump;
    [HideInInspector]
    public WolfAI ai;

    void Awake()
    {
        pilot = GetComponent<Pilot>();
        jump = GetComponent<Jump2D>();
        ai = GetComponent<WolfAI>();
    }

    // Actions ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;

    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowSwitch;

    // Event Manager ============================================================================

    void OnEnable()
    {
        EventManager.Current.TryMoveXEvent += OnTryMoveX;
        EventManager.Current.TryMoveYEvent += OnTryMoveY;
        EventManager.Current.TryJumpEvent += OnTryJump;
        EventManager.Current.TrySwitchEvent += OnTrySwitch;

        PlayerManager.Current.Register(gameObject);
    }
    void OnDisable()
    {
        EventManager.Current.TryMoveXEvent -= OnTryMoveX;
        EventManager.Current.TryMoveYEvent -= OnTryMoveY;
        EventManager.Current.TryJumpEvent -= OnTryJump;
        EventManager.Current.TrySwitchEvent -= OnTrySwitch;

        PlayerManager.Current.Unregister(gameObject);
    }

    // Events ============================================================================

    void OnTryMoveX(GameObject mover, float input_x)
    {
        if(mover!=gameObject) return;

        if(!AllowMoveX) return;

        EventManager.Current.OnMoveX(gameObject, input_x);
    }

    void OnTryMoveY(GameObject mover, float input_y)
    {
        if(mover!=gameObject) return;

        if(!AllowMoveY) return;

        EventManager.Current.OnMoveY(gameObject, input_y);
    }

    void OnTryJump(GameObject jumper, float input)
    {
        if(jumper!=gameObject) return;

        if(!AllowJump) return;

        EventManager.Current.OnJump(gameObject, input);
    }

    void OnTrySwitch(GameObject switcher)
    {
        if(switcher!=gameObject) return;

        if(!AllowSwitch) return;
        
        PlayerManager.Current.TrySwitch(gameObject);
    }

    // ============================================================================

    void Start()
    {
        EventManager.Current.OnSpawn(gameObject);
    }

    // ============================================================================

    public bool IsGrounded()
    {
        return jump.IsGrounded();
    }
    
}