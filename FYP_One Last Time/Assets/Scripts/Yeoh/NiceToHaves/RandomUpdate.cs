using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUpdate : SlowUpdate
{
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    
    // ============================================================================

    [Header("RandomUpdate")]
    public Vector2 randomInterval = new(.25f, 1);

    void RandomizeInterval()
    {
        checkInterval = Random.Range(randomInterval.x, randomInterval.y);

        OnRandomUpdate();
    }

    protected virtual void OnRandomUpdate(){}

    // ============================================================================

    void Start()
    {
        RandomizeInterval();
    }

    protected override void OnSlowUpdate()
    {
        RandomizeInterval();
    }
}
