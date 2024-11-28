using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEcho : SlowUpdate
{
    [Header("SpriteEcho")]
    public GameObject owner;
    public List<SpriteRenderer> sprites;

    // ============================================================================
    
    [Header("Echo")]
    public Vector3 echoPosOffset = new(0,0,.1f);
    public float echoLifetime=.25f;

    // ============================================================================
    
    public override void OnSlowUpdate()
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
        clone.hideFlags = HideFlags.HideInHierarchy;

        clone.transform.position = sprite_tr.position + echoPosOffset;
        clone.transform.localScale = Vector3.Scale(sprite_tr.localScale, owner.transform.localScale);
        clone.transform.rotation = sprite_tr.rotation;

        return clone;
    }

    void AddAndCopySprite(GameObject who, SpriteRenderer sr_source)
    {
        SpriteRenderer sr = who.AddComponent<SpriteRenderer>();

        sr.sprite = sr_source.sprite;
        sr.material = sr_source.material;

        Color color = sr_source.color;
        color.a = echoCurrentAlpha;
        sr.color = color;

        sr.flipX = sr_source.flipX;
        sr.flipY = sr_source.flipY;

        sr.sortingOrder = sr_source.sortingOrder-2;
    }

    void AddFadeAnim(GameObject who)
    {
        FadeAnim fade = who.AddComponent<FadeAnim>();
        fade.TweenAlpha(0, echoLifetime);
        Destroy(who, echoLifetime);
    }

    // ============================================================================

    [Header("Speed Check")]
    public VelocityMeter velM;
    public float minSpeed=15;
    public float maxSpeed=25;

    [Header("Opacity")]
    public float echoAlphaMin=.1f;
    public float echoAlphaMax=.75f;
    float echoCurrentAlpha;

    public override void OnFixedUpdate_su()
    {
        float speed = velM.velocityMagnitude;

        float speed_range = maxSpeed - minSpeed;
        float speed_offset = speed - minSpeed;
        float speed01 = speed_offset / speed_range;
        speed01 = Mathf.Clamp01(speed01);

        echoCurrentAlpha = Mathf.Lerp(echoAlphaMin, echoAlphaMax, speed01);
    }
}
