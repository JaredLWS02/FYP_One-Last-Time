using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class SquashStretchAnim : MonoBehaviour
{
    public GameObject owner;
    public Transform target;

    Vector3 defaultScale;

    void Awake()
    {
        defaultScale = target.localScale;
    }

    // ============================================================================

    [System.Serializable]
    public class ScaleAnim
    {
        public Vector3 scaleTo;
        public float seconds;
        public Ease ease;
    }

    // ============================================================================

    public List<ScaleAnim> jumpScaleAnims;
    public List<ScaleAnim> fallScaleAnims;
    public List<ScaleAnim> landScaleAnims;
    public List<ScaleAnim> dashScaleAnims;

    // ============================================================================

    [ContextMenu("Load Default Anims")]
    void LoadDefaults()
    {
        if(!target) return;
        
        defaultScale = target.localScale;

        jumpScaleAnims = new List<ScaleAnim>
        {
            new ScaleAnim
            {
                scaleTo = new(1.2f, 0.8f, 1.2f),
                seconds = 0.05f,
                ease = Ease.InSine,
            },
            new ScaleAnim
            {
                scaleTo = new(0.8f, 1.2f, 0.8f),
                seconds = 0.05f,
                ease = Ease.InSine,
            },
            new ScaleAnim
            {
                scaleTo = defaultScale,
                seconds = 0.2f,
                ease = Ease.OutSine,
            },
        };

        fallScaleAnims = new List<ScaleAnim>
        {
            new ScaleAnim
            {
                scaleTo = new(0.8f, 1.2f, 0.8f),
                seconds = 1,
                ease = Ease.InSine,
            },
        };

        landScaleAnims = new List<ScaleAnim>
        {
            new ScaleAnim
            {
                scaleTo = new(1.2f, 0.8f, 1.2f),
                seconds = 0.05f,
                ease = Ease.OutSine,
            },
            new ScaleAnim
            {
                scaleTo = defaultScale,
                seconds = 0.15f,
                ease = Ease.InOutSine,
            },
        };

        dashScaleAnims = new List<ScaleAnim>
        {
            new ScaleAnim
            {
                scaleTo = new(0.7f, 1.3f, 0.7f),
                seconds = 0.05f,
                ease = Ease.InSine,
            },
            new ScaleAnim
            {
                scaleTo = new(1.3f, 0.7f, 1.3f),
                seconds = 0.05f,
                ease = Ease.OutSine,
            },
            new ScaleAnim
            {
                scaleTo = defaultScale,
                seconds = 0.1f,
                ease = Ease.OutSine,
            },
        };
    }

    // ============================================================================
    
    Tween scaleTween;

    void TweenScale(Vector3 to, float time, Ease ease)
    {
        scaleTween.Stop();
        if(time>0) scaleTween = Tween.Scale(target, to, time, ease);
        else target.localScale = to;
    }

    // ============================================================================

    void PlayScaleAnims(List<ScaleAnim> scaleAnims)
    {
        TryStopCoroutine(PlayingScaleAnims_crt);
        PlayingScaleAnims_crt = StartCoroutine(PlayingScaleAnims(scaleAnims));
    }

    Coroutine PlayingScaleAnims_crt;

    void TryStopCoroutine(Coroutine crt)
    {
        if(crt!=null) StopCoroutine(crt);
    }

    IEnumerator PlayingScaleAnims(List<ScaleAnim> scaleAnims)
    {
        for(int i=0; i<scaleAnims.Count; i++)
        {
            TweenScale(scaleAnims[i].scaleTo, scaleAnims[i].seconds, scaleAnims[i].ease);

            yield return new WaitForSeconds(scaleAnims[i].seconds);
        }
    }

    // ============================================================================

    public void CancelAnims()
    {
        TryStopCoroutine(PlayingScaleAnims_crt);
        scaleTween.Stop();
        target.localScale = defaultScale;
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.JumpEvent += OnJump;
        EventM.AutoJumpEvent += OnAutoJump;
        //EventM.FastFallStartEvent += OnFallStart;
        //EventM.FastFallEndEvent += OnFallEnd;
        EventM.LandGroundEvent += OnLand;
        //EventM.DashEvent += OnDash;
    }
    void OnDisable()
    {
        EventM.JumpEvent -= OnJump;
        EventM.AutoJumpEvent -= OnAutoJump;
        //EventM.FastFallStartEvent -= OnFallStart;
        //EventM.FastFallEndEvent -= OnFallEnd;
        EventM.LandGroundEvent -= OnLand;
        //EventM.DashEvent -= OnDash;
    }
    
    // ============================================================================
    
    public void OnJump(GameObject jumper, float input)
    {
        if(jumper!=owner) return;

        if(input>0)
        // wait for OnFallEnd to cancel all first
        Invoke(nameof(Jump), 0.01f);
    }

    public void OnAutoJump(GameObject jumper, Vector3 jump_dir)
    {
        if(jumper!=owner) return;

        // wait for OnFallEnd to cancel all first
        Invoke(nameof(Jump), 0.01f);
    }

    public void Jump()
    {
        PlayScaleAnims(jumpScaleAnims);
    }

    // public void OnFallStart(GameObject who)
    // {
    //     if(who!=owner) return;

    //     PlayScaleAnims(fallScaleAnims);
    // }

    // public void OnFallEnd(GameObject who)
    // {
    //     if(who!=owner) return;

    //     print($"{owner.name}: fall ended");
    //     //CancelAnims();
    // }

    public void OnLand(GameObject who)
    {
        if(who!=owner) return;
        // wait for OnFallEnd to cancel all first
        Invoke(nameof(Land), 0.01f);
    }

    public void Land()
    {
        PlayScaleAnims(landScaleAnims);
    }

    public void OnDash(GameObject who)
    {
        if(who!=owner) return;

        // wait for OnFallEnd to cancel all first
        Invoke(nameof(Dash), 0.01f);
    }

    public void Dash()
    {
        PlayScaleAnims(dashScaleAnims);
    }

    // public float squashMult=.2f, squashTime=.1f;

    // public void fall()
    // {
    //     lt2 = LeanTween.scale(pivotBottom, new Vector2(defscalePivot.x*(1-squashMult),defscalePivot.y*(1+squashMult)),squashTime*7).setEaseInOutSine().id;
    // }
}
