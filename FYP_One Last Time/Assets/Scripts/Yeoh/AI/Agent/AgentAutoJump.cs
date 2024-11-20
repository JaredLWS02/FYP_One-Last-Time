using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AgentAutoJump : MonoBehaviour
{
    public GameObject owner;
    public NavMeshAgent agent;

    // ============================================================================

    void Update()
    {
        // use custom movement instead
        agent.autoTraverseOffMeshLink = false;
    }

    void FixedUpdate()
    {
        TryJump();
        UpdateJumpSpline();

        if(rb && isJumping)
        rb.isKinematic = true;
    }

    // ============================================================================

    void TryJump()
    {
        if(!agent.isOnOffMeshLink) return;
        if(isJumping) return;

        EventM.OnAgentTryAutoJump(owner, GetJumpDir());
    }

    Vector3 GetJumpDir()
    {
        Vector3 end_pos = agent.currentOffMeshLinkData.endPos;
        return (end_pos - owner.transform.position).normalized;
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.AutoJumpEvent += OnAutoJump;
        EventM.CancelAutoJumpEvent += OnCancelAutoJump;
    }
    void OnDisable()
    {
        EventM.AutoJumpEvent -= OnAutoJump;
        EventM.CancelAutoJumpEvent -= OnCancelAutoJump;
    }

    // ============================================================================

    void OnAutoJump(GameObject who, Vector3 jump_dir)
    {
        if(who!=owner) return;

        if(isJumping) return;

        if(IsCooling()) return;

        if(ground && !ground.IsGrounded()) return;

        StartJump();
    }

    // ============================================================================
    
    public bool isJumping {get; private set;}
    float jumpProgress=0;

    Vector3 startPos;
    Spline spline;
    bool isReversed;

    void StartJump()
    {
        isJumping=true;
        jumpProgress=0;

        if(rb) rb.isKinematic = true;

        NavMeshLink link = (NavMeshLink) agent.navMeshOwner;

        spline = link.GetComponent<Spline>();

        startPos = owner.transform.position;

        isReversed = IsJumpReversed(link);

        jumpAnim?.Play(owner);

        //toggler?.ToggleIgnoreLayers(true);

        EventM.OnJumped(owner);
        EventM.OnAutoJumped(owner, GetJumpDir());
    }

    // ============================================================================
    
    bool IsJumpReversed(NavMeshLink link)
    {
        // world positions
        Vector3 start_pos = link.transform.TransformPoint(link.startPoint);
        Vector3 end_pos = link.transform.TransformPoint(link.endPoint);

        // distances
        float owner_to_start = Vector3.Distance(owner.transform.position, start_pos);
        float owner_to_end = Vector3.Distance(owner.transform.position, end_pos);

        // if closer to end point than start point
        return owner_to_end < owner_to_start;
    }

    // ============================================================================

    [Header("Jump")]
    [Min(.01f)]
    public float jumpSeconds=.8f;
    public AnimSO jumpAnim;

    Vector3 jumpProgressPos;

    void UpdateJumpSpline()
    {
        if(!isJumping) return;

        jumpProgress += Time.fixedDeltaTime / jumpSeconds;

        float lerp01 = Mathf.Clamp01(jumpProgress);

        // invert value if reversed
        lerp01 = isReversed ? 1-lerp01 : lerp01;

        // move owner along spline
        jumpProgressPos = isReversed ?
            spline.CalcPosFromEnd(lerp01, startPos) :
            spline.CalcPosFromStart(lerp01, startPos);

        owner.transform.position = jumpProgressPos;

        EventM.OnAutoJumping(owner, GetJumpDir());

        if(jumpProgress>=1)
        {
            FinishJump();
        }
    }
    
    // ============================================================================

    [Header("Land")]
    public AnimSO landAnim;

    void FinishJump()
    {
        isJumping=false;
        jumpProgress=0;

        if(rb) rb.isKinematic = false;

        agent.CompleteOffMeshLink();

        landAnim?.Play(owner);

        //toggler?.ToggleIgnoreLayers(false);
        
        EventM.OnLandGround(owner);

        DoCooldown();
    }

    // ============================================================================

    [Header("After Land")]
    public Timer cooldown;
    public float cooldownTime=.5f;

    void DoCooldown() => cooldown?.StartTimer(cooldownTime);
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();

    // ============================================================================

    void OnCancelAutoJump(GameObject who)
    {
        if(who!=owner) return;
        if(!isJumping) return;

        FinishJump();
        owner.transform.position = jumpProgressPos;

        EventM.OnAutoJumpCancelled(owner);
    }

    // ============================================================================
    
    [Header("Optional")]
    public Rigidbody rb;
    public GroundCheck ground;
    public ColliderLayerToggler toggler;
}

// Tutorial by SunnyValleyStudio YouTube