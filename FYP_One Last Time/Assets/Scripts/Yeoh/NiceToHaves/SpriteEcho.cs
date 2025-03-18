using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEcho : SlowUpdate
{
    [Header("SpriteEcho")]
    public List<SpriteRenderer> sprites;

    public bool hideInHierarchy = true;

    // ============================================================================
    
    [Header("Echo")]
    public Vector3 localPos = new(0,0,.1f);
    public Vector3 localAngles = new(0,0,0);
    public Vector3 localScale = new(1,1,1);
    public float minLifetime=.1f;
    public float maxLifetime=.25f;

    // ============================================================================
    
    protected override void OnSlowUpdate()
    {
        float speed = velM.velocityMagnitude;
        
        if(speed >= minSpeed)
            SpawnEchos();
    }

    void SpawnEchos()
    {
        foreach(var sprite in sprites)
        {
            GameObject clone = SpawnClone(sprite.transform);
            AddAndCopySprite(clone, sprite);
            AddFadeAnim(clone);
        }
    }

    GameObject SpawnClone(Transform sprite_tr)
    {
        GameObject clone = new("Echo");
        if(hideInHierarchy)
        clone.hideFlags = HideFlags.HideInHierarchy;

        // temp parent to match its local transforms
        clone.transform.parent = sprite_tr;

        clone.transform.localScale = localScale;
        clone.transform.localEulerAngles = localAngles;
        clone.transform.localPosition = localPos;

        clone.transform.parent = null;

        return clone;
    }

    void AddAndCopySprite(GameObject who, SpriteRenderer sr_source)
    {
        SpriteRenderer sr = who.AddComponent<SpriteRenderer>();

        sr.sprite = sr_source.sprite;
        sr.material = sr_source.material;

        Color color = sr_source.color;
        color.a = currentAlpha;
        sr.color = color;

        sr.flipX = sr_source.flipX;
        sr.flipY = sr_source.flipY;

        sr.sortingOrder = sr_source.sortingOrder-1;
    }

    void AddFadeAnim(GameObject who)
    {
        float alpha01 = GetValue01(currentAlpha, minAlpha, maxAlpha);
        float lifetime = Mathf.Lerp(minLifetime, maxLifetime, alpha01);

        FadeAnim fade = who.AddComponent<FadeAnim>();
        fade.TweenAlpha(0, lifetime);
        Destroy(who, lifetime);
    }

    // ============================================================================

    [Header("Speed Check")]
    public VelocityMeter velM;
    public float minSpeed=15;
    public float maxSpeed=25;

    [Header("Opacity")]
    public float minAlpha=.1f;
    public float maxAlpha=.5f;
    float currentAlpha;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float speed = velM.velocityMagnitude;
        float speed01 = GetValue01(speed, minSpeed, maxSpeed);
        currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, speed01);
    }

    // ============================================================================

    float GetValue01(float current, float min, float max)
    {
        if(current <= min) return 0;
        if(current >= max) return 1;
        
        float range = max - min;
        if(range<=0) return 0;

        float offset = current - min;
        float value01 = offset / range;

        return Mathf.Clamp01(value01);
    }
}
