using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class MoveScript : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    public void UpdateMove(float speed_, Vector3 direction)
    {
        float min_speed = -Mathf.Min(speed, clamp);
        float max_speed = Mathf.Min(speed, clamp);

        // never go past speed nor clamp
        // also in the negatives
        speed_ = Mathf.Clamp(speed_, min_speed, max_speed);

        Move(speed_, direction);
    }

    public void UpdateMoveMult(float speed_mult, Vector3 direction)
    {
        // normalized between -1 and 1
        speed_mult = Mathf.Clamp(speed_mult, -1, 1);

        // choose speed or clamp
        float speed_ = Mathf.Min(speed, clamp);
        speed_ *= speed_mult;

        UpdateMove(speed_, direction);
    }

    public void UpdateMove(Vector3 velocity)
    {
        UpdateMove(velocity.magnitude, velocity.normalized);
    }

    // ============================================================================
    
    void Move(float speed, Vector3 direction)
    {
        direction.Normalize();

        float accelRate = Mathf.Abs(speed)>0 ? acceleration : deceleration; // use decelerate value if no input, and vice versa
    
        float speedDif = speed - Vector3.Dot(direction, rb.velocity); // difference between current and target speed

        float movement = Mathf.Abs(speedDif) * accelRate * Mathf.Sign(speedDif); // slow down or speed up depending on speed difference

        rb.AddForce(direction * movement);
    }
    
    // ============================================================================

    Tween speedTween;

    void TweenSpeed(float to, float time=.25f)
    {
        speedTween.Stop();
        if(time>0) speedTween = Tween.Custom(speed, to, time, onValueChange: newVal => speed=newVal, Ease.InOutSine);
        else speed = to;
    }

    Tween clampTween;

    void TweenClamp(float to, float time=.25f)
    {
        clampTween.Stop();
        if(time>0) clampTween = Tween.Custom(clamp, to, time, onValueChange: newVal => clamp=newVal, Ease.InOutSine);
        else clamp = to;
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

    public void Push(Vector3 velocity)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(velocity, ForceMode.Impulse);
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