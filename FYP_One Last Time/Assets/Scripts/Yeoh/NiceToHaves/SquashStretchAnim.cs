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
                scaleTo = new(1.2f, .8f, 1.2f),
                seconds = .05f,
                ease = Ease.InSine,
            },
            new ScaleAnim
            {
                scaleTo = new(.8f, 1.2f, .8f),
                seconds = .05f,
                ease = Ease.InSine,
            },
            new ScaleAnim
            {
                scaleTo = defaultScale,
                seconds = .2f,
                ease = Ease.OutSine,
            },
        };

        landScaleAnims = new List<ScaleAnim>
        {
            new ScaleAnim
            {
                scaleTo = new(1.2f, .8f, 1.2f),
                seconds = .05f,
                ease = Ease.OutSine,
            },
            new ScaleAnim
            {
                scaleTo = defaultScale,
                seconds = .15f,
                ease = Ease.InOutSine,
            },
        };

        dashScaleAnims = new List<ScaleAnim>
        {
            new ScaleAnim
            {
                scaleTo = new(.7f, 1.3f, .7f),
                seconds = .05f,
                ease = Ease.InSine,
            },
            new ScaleAnim
            {
                scaleTo = new(1.3f, .7f, 1.3f),
                seconds = .05f,
                ease = Ease.OutSine,
            },
            new ScaleAnim
            {
                scaleTo = defaultScale,
                seconds = .1f,
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
        if(PlayingScaleAnims_crt!=null) StopCoroutine(PlayingScaleAnims_crt);
        PlayingScaleAnims_crt = StartCoroutine(PlayingScaleAnims(scaleAnims));
    }

    Coroutine PlayingScaleAnims_crt;

    IEnumerator PlayingScaleAnims(List<ScaleAnim> scaleAnims)
    {
        for(int i=0; i<scaleAnims.Count; i++)
        {
            // if not first, wait for previous tween to finish
            if(i>0) yield return new WaitForSeconds(scaleAnims[i-1].seconds);

            TweenScale(scaleAnims[i].scaleTo, scaleAnims[i].seconds, scaleAnims[i].ease);
        }
    }

    // ============================================================================

    public void Cancel()
    {
        if(PlayingScaleAnims_crt!=null) StopCoroutine(PlayingScaleAnims_crt);
        scaleTween.Stop();
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.JumpEvent += OnJump;
        EventManager.Current.AutoJumpEvent += OnAutoJump;
        EventManager.Current.LandGroundEvent += OnLand;
        //EventManager.Current.DashEvent += OnDash;
    }
    void OnDisable()
    {
        EventManager.Current.JumpEvent -= OnJump;
        EventManager.Current.AutoJumpEvent -= OnAutoJump;
        EventManager.Current.LandGroundEvent -= OnLand;
        //EventManager.Current.DashEvent -= OnDash;
    }
    
    // ============================================================================
    
    public void OnJump(GameObject jumper, float input)
    {
        if(jumper!=owner) return;

        if(input>0) PlayScaleAnims(jumpScaleAnims);
    }

    public void OnAutoJump(GameObject jumper, Vector3 jump_dir)
    {
        if(jumper!=owner) return;

        PlayScaleAnims(jumpScaleAnims);
    }

    public void OnLand(GameObject who)
    {
        if(who!=owner) return;

        PlayScaleAnims(landScaleAnims);
    }

    public void OnDash(GameObject who)
    {
        if(who!=owner) return;

        PlayScaleAnims(dashScaleAnims);
    }

    // public float squashMult=.2f, squashTime=.1f;

    // public void fall()
    // {
    //     lt2 = LeanTween.scale(pivotBottom, new Vector2(defscalePivot.x*(1-squashMult),defscalePivot.y*(1+squashMult)),squashTime*7).setEaseInOutSine().id;
    // }
}
