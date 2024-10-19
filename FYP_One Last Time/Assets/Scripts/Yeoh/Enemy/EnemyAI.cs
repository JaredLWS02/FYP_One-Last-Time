using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pilot))]

[RequireComponent(typeof(SideMove))]
[RequireComponent(typeof(SideTurn))]
[RequireComponent(typeof(JumpScript))]
[RequireComponent(typeof(GroundCheck))]

[RequireComponent(typeof(Radar))]
[RequireComponent(typeof(AgentVelocity))]
[RequireComponent(typeof(AgentSideMove))]
[RequireComponent(typeof(AgentJump))]
[RequireComponent(typeof(AgentWander))]
[RequireComponent(typeof(AgentFlee))]

public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;

    SideMove move;
    SideTurn turn;
    JumpScript jump;
    GroundCheck ground;

    Radar radar;
    AgentVelocity agentV;
    AgentSideMove agentMove;
    AgentJump autoJump;
    AgentWander wander;
    AgentFlee flee;

    void Awake()
    {
        pilot = GetComponent<Pilot>();

        move = GetComponent<SideMove>();
        turn = GetComponent<SideTurn>();
        jump = GetComponent<JumpScript>();
        ground = GetComponent<GroundCheck>();

        radar = GetComponent<Radar>();
        agentV = GetComponent<AgentVelocity>();
        agentMove = GetComponent<AgentSideMove>();
        autoJump = GetComponent<AgentJump>();
        wander = GetComponent<AgentWander>();
        flee = GetComponent<AgentFlee>();

        if(agentV == null)
        {
            Debug.LogError("AgentVelocity component missing.");
        }
    }

    void Start()
    {
        EventManager.Current.OnSpawn(gameObject);
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.TryMoveXEvent += OnTryMoveX;
        EventManager.Current.TryFaceXEvent += OnTryFaceX;
        EventManager.Current.TryMoveYEvent += OnTryMoveY;
        EventManager.Current.TryJumpEvent += OnTryJump;
        EventManager.Current.TryAutoJumpEvent += OnTryAutoJump;
    }
    void OnDisable()
    {
        EventManager.Current.TryMoveXEvent -= OnTryMoveX;
        EventManager.Current.TryFaceXEvent -= OnTryFaceX;
        EventManager.Current.TryMoveYEvent -= OnTryMoveY;
        EventManager.Current.TryJumpEvent -= OnTryJump;
        EventManager.Current.TryAutoJumpEvent -= OnTryAutoJump;
    }

    // ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;

    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowAutoJump;

    // ============================================================================

    void OnTryMoveX(GameObject who, float input_x)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowMoveX) return;

        EventManager.Current.OnMoveX(gameObject, input_x);

        move.OnMove(input_x);
    }

    void OnTryFaceX(GameObject who, float input_x)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowMoveX) return;

        EventManager.Current.OnFaceX(gameObject, input_x);

        turn.TryFlip(input_x);
    }

    void OnTryMoveY(GameObject who, float input_y)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowMoveY) return;

        EventManager.Current.OnMoveY(gameObject, input_y); // send to one way platform
    }

    // ============================================================================
    
    void OnTryJump(GameObject who, float input)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowJump) return;

        EventManager.Current.OnJump(gameObject, input);

        jump.OnJump(input);
    }

    void OnTryAutoJump(GameObject who)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowAutoJump) return;

        EventManager.Current.OnAutoJump(gameObject);

        autoJump.StartJump();
    }

    // ============================================================================
    
    public bool IsGrounded()
    {
        return ground.IsGrounded();
    }

    public bool IsAutoJumping()
    {
        return autoJump.isJumping;
    }

    [Header("HP")]
    public HPManager hp;
    public float healthyMinPercent=50;

    public bool IsHealthy()
    {
        if(!hp) return true;

        return hp.GetHPPercent() >= healthyMinPercent;
    }

    // ============================================================================

    [Header("Radar")]
    public string enemyTag = "Player";
    public string lootTag = "Loot";

    public GameObject GetClosest(string tag)
    {
        return radar.GetClosestTargetWithTag(tag);
    }

    public GameObject GetEnemy()
    {
        return GetClosest(enemyTag);
    }

    public GameObject GetLoot()
    {
        return GetClosest(lootTag);
    }

    // ============================================================================

    [Header("Ranges")]
    public float shortRange=1;
    public float mediumRange=3;
    public float longRange=5;

    public float GetCurrentRange()
    {
        return agentV.stoppingRange;
    }

    public void SetRange(float to)
    {
        if(agentV == null)
        {
            Debug.LogError("AgentVelocity is null. Cannot set range.");
            return;
        }
        
        agentV.stoppingRange = to;
    }

    public bool IsInRange(GameObject target, float range)
    {
        if(!target) return false;
        return Vector2.Distance(transform.position, target.transform.position) <= range;
    }

    public bool IsInRange(GameObject target)
    {
        return IsInRange(target, GetCurrentRange());
    }

    public bool IsInRange()
    {
        return IsInRange(GetCurrentGoal(), GetCurrentRange());
    }

    // ============================================================================

    public GameObject GetCurrentGoal()
    {
        return agentV.goal.gameObject;
    }

    public void SetGoal(Transform target)
    {
        agentV.goal = target;
    }

    public void SetGoal(GameObject target)
    {
        SetGoal(target.transform);
    }

    public void SetGoal(GameObject target, float range)
    {
        SetRange(range);
        SetGoal(target);
    }

    public void SetGoalWander()
    {
        SetRange(shortRange);
        SetGoal(wander.goal);
    }

    public void SetGoalEnemy()
    {
        SetRange(mediumRange);
        SetGoal(GetEnemy());
    }

    public void SetGoalFlee()
    {
        SetRange(shortRange);
        SetGoal(flee.goal);
    }

    // ============================================================================

    public bool IsFacingTarget(Transform target)
    {
        return (target.position.x >= transform.position.x && turn.faceR)
            || (target.position.x < transform.position.x && !turn.faceR);
    }

    public void FaceTarget(GameObject target)
    {
        if(IsFacingTarget(target.transform)) return;

        float x_dir = turn.faceR ? -1 : 1;

        EventManager.Current.OnTryFaceX(gameObject, x_dir);
    }

    public void FaceGoal()
    {
        FaceTarget(GetCurrentGoal());
    }

    // ============================================================================

    public GameObject GetThreat()
    {
        return flee.threat.gameObject;
    }

    public void SetThreat(Transform target)
    {
        flee.threat = target;
        SetGoalFlee();
    }

    public void SetThreat(GameObject target)
    {
        SetThreat(target.transform);
    }

    public void SetThreatEnemy()
    {
        SetThreat(GetEnemy());
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColor = new(1, 1, 1, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, shortRange);
        Gizmos.DrawWireSphere(transform.position, mediumRange);
        Gizmos.DrawWireSphere(transform.position, longRange);
    }
}
