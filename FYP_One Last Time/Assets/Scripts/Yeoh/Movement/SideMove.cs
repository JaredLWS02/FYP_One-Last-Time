using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMove : MoveScript
{    
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.MoveEvent += OnMove;
    }
    void OnDisable()
    {
        EventM.MoveEvent -= OnMove;
    }

    // ============================================================================

    float inputX;

    void OnMove(GameObject who, Vector2 input)
    {
        if(who!=owner) return;

        inputX = input.x;
    }
    
    // ============================================================================

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        inputX = Round(inputX, 1);

        UpdateMoveMult(inputX, Vector3.right);
    }
}
