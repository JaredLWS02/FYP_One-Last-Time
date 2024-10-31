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

        UpdateCooldown();
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
    }
    void OnDisable()
    {
        EventM.AutoJumpEvent -= OnAutoJump;
    }

    // ============================================================================

    void OnAutoJump(GameObject who, Vector3 jump_dir)
    {
        if(who!=owner) return;

        if(isJumping) return;

        if(IsCooling()) return;
        DoCooldown();

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

    void UpdateJumpSpline()
    {
        if(!isJumping) return;

        jumpProgress += Time.fixedDeltaTime / jumpSeconds;

        float lerp01 = Mathf.Clamp01(jumpProgress);

        // invert value if reversed
        lerp01 = isReversed ? 1-lerp01 : lerp01;

        // move owner along spline
        owner.transform.position = isReversed
            ? spline.CalcPosFromEnd(lerp01, startPos)
            : spline.CalcPosFromStart(lerp01, startPos);

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
        
        EventM.OnLandGround(owner);
    }

    // ============================================================================

    public float cooldownTime=.5f;
    float cooldownLeft;
    
    void DoCooldown() => cooldownLeft = cooldownTime;

    void UpdateCooldown()
    {
        // only tick down if not jumping
        if(isJumping) return;
        
        cooldownLeft -= Time.deltaTime;

        if(cooldownLeft<0) cooldownLeft=0;
    }

    bool IsCooling() => cooldownLeft>0;

    void CancelCooldown() => cooldownLeft=0;

    // ============================================================================
    
    [Header("Optional")]
    public Rigidbody rb;
}

// Tutorial by SunnyValleyStudio YouTube