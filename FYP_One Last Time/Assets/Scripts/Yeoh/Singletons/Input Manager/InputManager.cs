using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class InputManager : MonoBehaviour
{
    public static InputManager Current;

    void Awake()
    {
        if(!Current) Current=this;

        SetupInputActions();
    }

    // ============================================================================

    PlayerInput playerInput;

    InputAction move;
    InputAction jump;
    InputAction dash;
    InputAction lightAttack;
    InputAction heavyAttack;
    InputAction parry;
    InputAction ability1;
    InputAction ability2;
    InputAction ability3;
    InputAction pullVines;
    InputAction reloadScene;
    InputAction mainMenuScene;
    InputAction pause;

    void SetupInputActions()
    {
        playerInput = GetComponent<PlayerInput>();

        move = playerInput.actions["Move"];
        jump = playerInput.actions["Jump"];
        dash = playerInput.actions["Dash"];
        lightAttack = playerInput.actions["Light Attack"];
        heavyAttack = playerInput.actions["Heavy Attack"];
        parry = playerInput.actions["Parry"];
        ability1 = playerInput.actions["Ability 1"];
        ability2 = playerInput.actions["Ability 2"];
        ability3 = playerInput.actions["Ability 3"];
        pullVines = playerInput.actions["Pull Vines"];
        reloadScene = playerInput.actions["Reload Scene"];
        mainMenuScene = playerInput.actions["MainMenu Scene"];
        pause = playerInput.actions["Pause"];
    }

    // ============================================================================

    public Vector2 moveAxis {get; private set;}
    public bool jumpKeyDown {get; private set;}
    public bool jumpKeyUp {get; private set;}
    public bool dashKeyDown {get; private set;}
    public bool lightAttackKeyDown {get; private set;}
    public bool heavyAttackKeyDown {get; private set;}
    public bool parryKeyDown {get; private set;}
    public bool ability1KeyDown {get; private set;}
    public bool ability2KeyDown {get; private set;}
    public bool ability3KeyDown {get; private set;}
    public bool pullVinesKeyDown {get; private set;}
    public bool reloadSceneKeyDown {get; private set;}
    public bool mainMenuSceneKeyDown {get; private set;}
    public bool pauseKeyDown {get; private set;}

    void Update()
    {
        moveAxis = move.ReadValue<Vector2>();
        jumpKeyDown = jump.WasPressedThisFrame();
        jumpKeyUp = jump.WasReleasedThisFrame();
        dashKeyDown = dash.WasPressedThisFrame();
        lightAttackKeyDown = lightAttack.WasPressedThisFrame();
        heavyAttackKeyDown = heavyAttack.WasPressedThisFrame();
        parryKeyDown = parry.WasPressedThisFrame();
        ability1KeyDown = ability1.WasPressedThisFrame();
        ability2KeyDown = ability2.WasPressedThisFrame();
        ability3KeyDown = ability3.WasPressedThisFrame();
        pullVinesKeyDown = pullVines.WasPressedThisFrame();
        reloadSceneKeyDown = reloadScene.WasPressedThisFrame();
        mainMenuSceneKeyDown = mainMenuScene.WasPressedThisFrame();
        pauseKeyDown = pause.WasPressedThisFrame();
    }
}
