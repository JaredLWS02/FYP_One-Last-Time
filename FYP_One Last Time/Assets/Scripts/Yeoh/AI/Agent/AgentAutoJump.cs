using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]

public class AgentAutoJump : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    NavMeshAgent agent;
    Rigidbody rb; // optional

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

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

        EventM.OnAgentTryAutoJump(gameObject, GetJumpDir());
    }

    Vector3 GetJumpDir()
    {
        Vector3 end_pos = agent.currentOffMeshLinkData.endPos;
        return (end_pos - agent.transform.position).normalized;
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
    
    [HideInInspector]
    public bool isJumping;
    float jumpProgress=0;

    Vector3 agentStartPos;
    Spline spline;
    bool isReversed;

    void StartJump()
    {
        isJumping=true;
        jumpProgress=0;

        if(rb) rb.isKinematic = true;

        NavMeshLink link = (NavMeshLink) agent.navMeshOwner;

        spline = link.GetComponent<Spline>();

        agentStartPos = agent.transform.position;

        isReversed = IsJumpReversed(link);

        OnJump?.Invoke();
        EventM.OnAutoJumped(gameObject, GetJumpDir());
    }

    // ============================================================================
    
    bool IsJumpReversed(NavMeshLink link)
    {
        // world positions
        Vector3 start_pos = link.transform.TransformPoint(link.startPoint);
        Vector3 end_pos = link.transform.TransformPoint(link.endPoint);

        // distances
        float agent_to_start = Vector3.Distance(agent.transform.position, start_pos);
        float agent_to_end = Vector3.Distance(agent.transform.position, end_pos);

        // if closer to end point than start point
        return agent_to_end < agent_to_start;
    }

    // ============================================================================

    [Min(.01f)]
    public float jumpSeconds=.8f;

    void UpdateJumpSpline()
    {
        if(!isJumping) return;

        jumpProgress += Time.fixedDeltaTime / jumpSeconds;

        float lerp01 = Mathf.Clamp01(jumpProgress);

        // invert value if reversed
        lerp01 = isReversed ? 1-lerp01 : lerp01;

        // move agent along spline
        agent.transform.position = isReversed
            ? spline.CalcPosFromEnd(lerp01, agentStartPos)
            : spline.CalcPosFromStart(lerp01, agentStartPos);

        if(jumpProgress>=1)
        {
            FinishJump();
        }
    }
    
    // ============================================================================

    void FinishJump()
    {
        isJumping=false;
        jumpProgress=0;

        if(rb) rb.isKinematic = false;

        agent.CompleteOffMeshLink();

        OnLand?.Invoke();
        EventM.OnLandGround(gameObject);
    }

    // ============================================================================

    public float cooldownTime=.5f;
    float cooldownLeft;
    
    void DoCooldown()
    {
        cooldownLeft = cooldownTime;
    }

    void UpdateCooldown()
    {
        // only tick down if not jumping
        if(isJumping) return;
        
        cooldownLeft -= Time.deltaTime;

        if(cooldownLeft<0) cooldownLeft=0;
    }

    bool IsCooling()
    {
        return cooldownLeft>0;
    }

    void CancelCooldown()
    {
        cooldownLeft=0;
    }

    // ============================================================================

    [Header("Events")]
    public UnityEvent OnJump;
    public UnityEvent OnLand;
}

// Tutorial by SunnyValleyStudio YouTube