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

    float dirX;

    void OnMove(GameObject who, Vector2 input)
    {
        if(who!=owner) return;

        dirX = input.x;
    }
    
    // ============================================================================

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        dirX = Round(dirX, 1);

        UpdateMoveMult(dirX, Vector3.right);
    }
}
