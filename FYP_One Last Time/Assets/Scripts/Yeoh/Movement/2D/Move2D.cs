using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Move2D : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseSpeed = clamp = speed;
    }

    // ============================================================================

    [Header("Move")]
    public float speed=10;
    [HideInInspector]
    public float baseSpeed;
    float clamp;

    public float acceleration=10;
    public float deceleration=10;

    // ============================================================================

    public void UpdateMove(float speed, Vector3 direction)
    {
        float accelRate = Mathf.Abs(speed)>0 ? acceleration : deceleration; // use decelerate value if no input, and vice versa
    
        float speedDif = speed - Vector3.Dot(direction, rb.velocity); // difference between current and target speed

        float movement = Mathf.Abs(speedDif) * accelRate * Mathf.Sign(speedDif); // slow down or speed up depending on speed difference

        rb.AddForce(direction * movement);
    }

    public void UpdateMove(Vector3 direction)
    {
        float speed_ = Mathf.Min(speed, clamp);
        UpdateMove(speed_, direction);
    }

    // ============================================================================

    Tween speedTween;

    public void TweenSpeed(float to, float time=.25f)
    {
        speedTween.Stop();
        speedTween = Tween.Custom(speed, to, time, onValueChange: newVal => speed=newVal, Ease.InOutSine);
    }

    Tween clampTween;

    void TweenClamp(float to, float time=.25f)
    {
        clampTween.Stop();
        clampTween = Tween.Custom(clamp, to, time, onValueChange: newVal => clamp=newVal, Ease.InOutSine);
    }

    // ============================================================================

    public void SetSpeed(float to, float tweenTime=.25f)
    {
        if(to==speed) return;

        if(to>=baseSpeed)
        {
            TweenSpeed(to, tweenTime);
            TweenClamp(baseSpeed, tweenTime);
        }
        else
        {
            TweenSpeed(baseSpeed, tweenTime);
            TweenClamp(to, tweenTime);
        }
    }

    public void ResetSpeed(float tweenTime=.25f)
    {
        SetSpeed(baseSpeed, tweenTime);
    }

    // ============================================================================

    public void Push(Vector3 vector)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(vector, ForceMode2D.Impulse);
    }

    // ============================================================================

    [Header("Debug")]
    public float velocity;

    void FixedUpdate()
    {
        velocity = Round(rb.velocity.magnitude, 2);
    }

    public float Round(float num, int decimalPlaces)
    {
        int factor=1;

        for(int i=0; i<decimalPlaces; i++)
        {
            factor *= 10;
        }

        return Mathf.Round(num * factor) / factor;
    }
}