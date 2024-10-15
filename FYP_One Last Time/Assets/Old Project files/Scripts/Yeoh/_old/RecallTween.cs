using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MnKMovement))]

public class RecallTween : MonoBehaviour
{
    MnKMovement movement;

    void Awake()
    {
        movement = GetComponent<MnKMovement>();
    }

    // ============================================================================

    bool canRecall = true;

    public Transform wolfTr;
    public float tweenSpeed = 4;
    public float cooldownTime = 1;

    Tween recallTween;

    // Input System ============================================================================

    void OnInputRecall()
    {
        // ignore if not grounded 
        if(!movement.IsGrounded()) return;

        // ignore if cooling down
        if(!canRecall) return;
        StartCoroutine(RecallCooling(cooldownTime));

        // restart if still tweening
        if(recallTween!=null && recallTween.IsActive())
        {
            recallTween.Kill();
        }

        recallTween = wolfTr.DOMove(transform.position, tweenSpeed)
            .SetEase(Ease.InOutSine)
            .SetSpeedBased(true);
    }

    // ============================================================================

    IEnumerator RecallCooling(float t)
    {
        canRecall = false;
        yield return new WaitForSeconds(t);
        canRecall = true;
    }
}
