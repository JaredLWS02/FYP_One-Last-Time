using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Pilot))]

[RequireComponent(typeof(SideMove))]
[RequireComponent(typeof(SideTurn))]
[RequireComponent(typeof(JumpScript))]
[RequireComponent(typeof(GroundCheck))]

[RequireComponent(typeof(NavMeshAgent))]
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

    NavMeshAgent agent;
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

        agent = GetComponent<NavMeshAgent>();
        radar = GetComponent<Radar>();
        agentV = GetComponent<AgentVelocity>();
        agentMove = GetComponent<AgentSideMove>();
        autoJump = GetComponent<AgentJump>();
        wander = GetComponent<AgentWander>();
        flee = GetComponent<AgentFlee>();
    }

    public Transform spawnpoint;

    void Start()
    {
        EventManager.Current.OnSpawn(gameObject);

        spawnpoint.parent = null;
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

    void OnTryAutoJump(GameObject who, Vector3 jump_dir)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowAutoJump) return;

        EventManager.Current.OnAutoJump(gameObject, jump_dir);

        autoJump.StartJump();

        float dot_x = Vector3.Dot(jump_dir, Vector3.right);

        turn.TryFlip(dot_x);
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

    [Header("HP Check")]
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
    public float arrivalRange=1;
    public float meleeRange=3;

    public float GetCurrentRange()
    {
        return agentV.stoppingRange;
    }

    public void SetRange(float to)
    {        
        agentV.stoppingRange = to;
    }

    public bool IsInRange(Vector3 from, Vector3 target, float range)
    {
        return Vector2.Distance(from, target) <= range;
    }

    public bool IsInRange(GameObject target, float range)
    {
        if(!target) return false;
        return IsInRange(transform.position, target.transform.position, range);
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
    
    public float maintainDistance=2;

    public bool IsEnemyTooClose()
    {
        return IsInRange(GetEnemy(), maintainDistance);
    }

    // ============================================================================
    
    public float maxChaseDownRange=7;
    public float returnRange=20;

    public bool ShouldReturn()
    {
        // ignore if cant find a way back
        if(!IsPathable(spawnpoint.position)) return false;

        // ignore if still close to enemy
        if(IsInRange(GetEnemy(), maxChaseDownRange)) return false;

        return !IsInRange(spawnpoint.position, transform.position, returnRange);
    }

    bool IsPathable(Vector3 pos)
    {
        NavMeshPath path = new();
        agent.CalculatePath(pos, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    public bool IsAtSpawnpoint()
    {
        return IsInRange(spawnpoint.position, transform.position, arrivalRange);
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
        if(!target) return;

        SetGoal(target.transform);
    }

    public void SetGoal(GameObject target, float range)
    {
        SetRange(range);
        SetGoal(target);
    }

    public void SetGoalWander()
    {
        SetRange(arrivalRange);
        SetGoal(wander.goal);
    }

    public void SetGoalEnemy()
    {
        SetRange(meleeRange);
        SetGoal(GetEnemy());
    }

    public void SetGoalFlee()
    {
        SetRange(arrivalRange);
        SetGoal(flee.goal);
    }

    public bool IsFleeing()
    {
        return GetCurrentGoal()==flee.goal.gameObject;
    }

    public void SetGoalSpawnpoint()
    {
        SetRange(arrivalRange);
        SetGoal(spawnpoint);
    }

    // ============================================================================

    public bool IsFacingTarget(Transform target)
    {
        return (target.position.x >= transform.position.x && turn.faceR)
            || (target.position.x < transform.position.x && !turn.faceR);
    }

    public void FaceTarget(GameObject target)
    {
        if(!target) return;

        if(IsFacingTarget(target.transform)) return;

        float x_dir = turn.faceR ? -1 : 1;

        EventManager.Current.OnTryFaceX(gameObject, x_dir);
    }

    public void FaceGoal()
    {
        FaceTarget(GetCurrentGoal());
    }

    public void FaceEnemy()
    {
        FaceTarget(GetEnemy());
    }

    // ============================================================================

    public GameObject GetThreat()
    {
        return flee.threat.gameObject;
    }

    public void SetThreat(Transform target)
    {
        if(!target) return;

        flee.threat = target;
        SetGoalFlee();
    }

    public void SetThreat(GameObject target)
    {
        if(!target) return;
        
        SetThreat(target.transform);
    }

    public void SetThreatEnemy()
    {
        SetThreat(GetEnemy());
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos = true;
    
    public bool showArrivalRangeGizmo = true;
    public bool showMeleeRangeGizmo = true;
    public bool showMaintainDistanceGizmo = true;
    public bool showMaxChaseDownRangeGizmo = true;
    public bool showReturnRangeGizmo = true;

    public Color gizmoColor = new(1, 1, 1, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;

        if(showArrivalRangeGizmo)
        Gizmos.DrawWireSphere(transform.position, arrivalRange);

        if(showMeleeRangeGizmo)
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        if(showMaintainDistanceGizmo)
        Gizmos.DrawWireSphere(transform.position, maintainDistance);

        if(showMaxChaseDownRangeGizmo)
        Gizmos.DrawWireSphere(transform.position, maxChaseDownRange);

        if(showReturnRangeGizmo)
        {
            Vector3 spawn_pos = Application.isPlaying ? spawnpoint.position : transform.position;
            Gizmos.DrawWireSphere(spawn_pos, returnRange);
        }
    }
}
