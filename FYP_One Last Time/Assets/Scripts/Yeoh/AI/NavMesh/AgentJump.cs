using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(TurnScript))]

public class AgentJump : MonoBehaviour
{
    NavMeshAgent agent;
    TurnScript turn;
    Rigidbody rb; // optional

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        turn = GetComponent<TurnScript>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // use custom movement instead
        agent.autoTraverseOffMeshLink = false;
    }

    // ============================================================================

    void FixedUpdate()
    {
        CheckJump();
        UpdateJump();
        UpdateFaceJumpDir();

        if(rb && isJumping)
        rb.isKinematic = true;
    }

    void CheckJump()
    {
        if(!agent.isOnOffMeshLink) return;
        if(isJumping) return;

        EventManager.Current.OnTryAutoJump(gameObject);
    }

    // ============================================================================

    [HideInInspector]
    public bool isJumping;
    float jumpProgress=0;

    Vector3 agentStartPos;
    Spline spline;
    bool isReversed;

    public void StartJump()
    {
        if(isJumping) return;

        isJumping=true;
        jumpProgress=0;

        if(rb)
        rb.isKinematic = true;

        NavMeshLink link = (NavMeshLink) agent.navMeshOwner;

        spline = link.GetComponent<Spline>();

        agentStartPos = agent.transform.position;

        isReversed = IsJumpReversed(link);

        OnJump?.Invoke();
    }

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

    void UpdateJump()
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

    void UpdateFaceJumpDir()
    {
        if(!isJumping) return;

        Vector3 end_pos = agent.currentOffMeshLinkData.endPos;

        Vector3 dir = (end_pos - agent.transform.position).normalized;

        turn.UpdateTurn(dir);
    }
    
    // ============================================================================

    void FinishJump()
    {
        isJumping=false;
        jumpProgress=0;

        if(rb)
        rb.isKinematic = false;

        agent.CompleteOffMeshLink();

        OnLand?.Invoke();
    }

    // ============================================================================

    public UnityEvent OnJump;
    public UnityEvent OnLand;
}

// Tutorial by SunnyValleyStudio YouTube