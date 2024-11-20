using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUpdate : SlowUpdate
{
    [Header("RandomUpdate")]
    public Vector2 randomInterval = new(.25f, 1);

    void RandomizeInterval()
    {
        checkInterval = Random.Range(randomInterval.x, randomInterval.y);

        OnRandomUpdate();
    }

    public virtual void OnRandomUpdate(){}

    // ============================================================================

    void Start()
    {
        RandomizeInterval();
    }

    public override void OnSlowUpdate()
    {
        RandomizeInterval();
    }
}
