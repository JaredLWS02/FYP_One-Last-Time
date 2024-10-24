using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Pilot))]

[RequireComponent(typeof(SideMove))]
[RequireComponent(typeof(SideTurn))]
[RequireComponent(typeof(JumpScript))]
[RequireComponent(typeof(GroundCheck))]

[RequireComponent(typeof(Radar))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentVelocity))]
[RequireComponent(typeof(AgentSideMove))]
[RequireComponent(typeof(AgentJump))]
[RequireComponent(typeof(AgentWander))]
[RequireComponent(typeof(AgentFlee))]
[RequireComponent(typeof(AgentReturn))]

[RequireComponent(typeof(AttackScript))]

public class AgentAI : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;

    SideMove move;
    SideTurn turn;
    JumpScript jump;
    GroundCheck ground;

    Radar radar;
    NavMeshAgent agent;
    AgentVelocity agentV;
    AgentSideMove agentMove;
    AgentJump autoJump;
    AgentWander wander;
    AgentFlee flee;
    AgentReturn returner;

    AttackScript attack;

    void Awake()
    {
        pilot = GetComponent<Pilot>();

        move = GetComponent<SideMove>();
        turn = GetComponent<SideTurn>();
        jump = GetComponent<JumpScript>();
        ground = GetComponent<GroundCheck>();

        radar = GetComponent<Radar>();
        agent = GetComponent<NavMeshAgent>();
        agentV = GetComponent<AgentVelocity>();
        agentMove = GetComponent<AgentSideMove>();
        autoJump = GetComponent<AgentJump>();
        wander = GetComponent<AgentWander>();
        flee = GetComponent<AgentFlee>();
        returner = GetComponent<AgentReturn>();

        attack = GetComponent<AttackScript>();
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

        EventManager.Current.TryAttackEvent += OnTryAttack;
    }
    void OnDisable()
    {
        EventManager.Current.TryMoveXEvent -= OnTryMoveX;
        EventManager.Current.TryFaceXEvent -= OnTryFaceX;
        EventManager.Current.TryMoveYEvent -= OnTryMoveY;
        EventManager.Current.TryJumpEvent -= OnTryJump;
        EventManager.Current.TryAutoJumpEvent -= OnTryAutoJump;

        EventManager.Current.TryAttackEvent -= OnTryAttack;
    }

    // ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;

    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowAutoJump;
    public bool AllowAttack;

    // ============================================================================

    void OnTryMoveX(GameObject who, float input_x)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowMoveX) return;

        EventManager.Current.OnMoveX(gameObject, input_x);

        move.UpdateMove(input_x);
    }

    void OnTryFaceX(GameObject who, float input_x)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowMoveX) return;

        EventManager.Current.OnFaceX(gameObject, input_x);

        turn.UpdateFlip(input_x);
    }

    void OnTryMoveY(GameObject who, float input_y)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowMoveY) return;

        EventManager.Current.OnMoveY(gameObject, input_y); // send to one way platform
    }
    
    void OnTryJump(GameObject who, float input)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowJump) return;

        jump.OnJump(input);
    }

    void OnTryAutoJump(GameObject who, Vector3 jump_dir)
    {
        if(!pilot.IsAI()) return;

        if(who!=gameObject) return;

        if(!AllowAutoJump) return;

        autoJump.StartJump();

        float dot_x = Vector3.Dot(jump_dir, Vector3.right);

        turn.UpdateFlip(dot_x);
    }

    // ============================================================================
    
    [Header("Attacks")]
    public AttackCombo normalCombo;
    
    void OnTryAttack(GameObject who, string type)
    {
        if(who!=gameObject) return;

        if(!AllowAttack) return;

        switch(type)
        {
            default: normalCombo.DoBuffer(); break;
        }
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

    public bool ShouldReturn()
    {
        Vector3 enemy_pos = GetEnemy().transform.position;
        return returner.ShouldReturn(enemy_pos);
    }

    public bool IsAtSpawnpoint()
    {
        return returner.IsAtSpawnpoint(agentV.stoppingRange);
    }

    public bool IsAttacking()
    {
        return attack.isAttacking;
    }
    
    // ============================================================================

    [Header("HP Check")]
    public HPManager hpM;
    public float healthyMinPercent=50;

    public bool IsHealthy()
    {
        if(!hpM) return true;

        return hpM.GetHPPercent() >= healthyMinPercent;
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
    public float attackRange=3;

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
        SetRange(attackRange);
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
        SetGoal(returner.spawnpoint);
    }

    // ============================================================================

    public void FaceMoveDir()
    {
        float dot_x = Vector3.Dot(Vector3.right, agentV.velocity);

        EventManager.Current.OnTryFaceX(gameObject, dot_x);
    }

    public void FaceTarget(GameObject target)
    {
        if(!target) return;

        Vector3 agent_to_target = (target.transform.position - agent.transform.position).normalized;

        float dot_x = Vector3.Dot(Vector3.right, agent_to_target);

        EventManager.Current.OnTryFaceX(gameObject, dot_x);
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

    public Color gizmoColor = new(1, 1, 1, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;

        if(showArrivalRangeGizmo)
        Gizmos.DrawWireSphere(transform.position, arrivalRange);

        if(showMeleeRangeGizmo)
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if(showMaintainDistanceGizmo)
        Gizmos.DrawWireSphere(transform.position, maintainDistance);
    }
}
