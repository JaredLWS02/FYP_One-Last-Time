using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Pilot))]

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentVelocity))]
[RequireComponent(typeof(AgentSideMove))]
[RequireComponent(typeof(AgentAutoJump))]
[RequireComponent(typeof(AgentWander))]
[RequireComponent(typeof(AgentFlee))]
[RequireComponent(typeof(AgentReturn))]
[RequireComponent(typeof(AgentVerticalityCheck))]

public class AgentManager : MonoBehaviour
{
    public Pilot pilot {get; private set;}

    NavMeshAgent agent;
    AgentVelocity agentVel;
    AgentWander wander;
    AgentFlee flee;
    AgentReturn returner;

    void Awake()
    {
        pilot = GetComponent<Pilot>();

        agent = GetComponent<NavMeshAgent>();
        agentVel = GetComponent<AgentVelocity>();
        wander = GetComponent<AgentWander>();
        flee = GetComponent<AgentFlee>();
        returner = GetComponent<AgentReturn>();
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================
    
    void Start()
    {
        EventM.OnSpawned(gameObject);
    }

    // ============================================================================
    
    [Header("HP Check")]
    public HPManager hpM;
    public float okHPPercent=50;
    public float lowHPPercent=25;

    public bool IsFullHP()
    {
        return hpM.GetHPPercent() >= 100;
    }
    public bool IsOkHP()
    {
        return hpM.GetHPPercent() >= okHPPercent;
    }
    public bool IsLowHP()
    {
        return hpM.GetHPPercent() <= lowHPPercent;
    }
    
    // ============================================================================

    [Header("Radar")]
    public Radar radar;
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
        return agentVel.stoppingRange;
    }

    public void SetRange(float to)
    {        
        agentVel.stoppingRange = to;
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
        return agentVel.goal.gameObject;
    }

    public void SetGoal(Transform target)
    {
        agentVel.goal = target;
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
        SetGoal(wander.wanderGoal);
    }

    public void SetGoalEnemy()
    {
        SetRange(attackRange);
        SetGoal(GetEnemy());
    }

    public void SetGoalFlee()
    {
        SetRange(arrivalRange);
        SetGoal(flee.fleeGoal);
    }

    public bool IsFleeing()
    {
        return GetCurrentGoal()==flee.fleeGoal.gameObject;
    }

    public void SetGoalSpawnpoint()
    {
        SetRange(arrivalRange);
        SetGoal(returner.spawnpoint);
    }

    // ============================================================================

    public void FaceMoveDir()
    {
        float dot_x = Vector3.Dot(Vector3.right, agentVel.velocity);

        EventM.OnAgentTryFaceX(gameObject, dot_x);
    }

    public void FaceTarget(GameObject target)
    {
        if(!target) return;

        Vector3 agent_to_target = (target.transform.position - agent.transform.position).normalized;

        float dot_x = Vector3.Dot(Vector3.right, agent_to_target);

        EventM.OnAgentTryFaceX(gameObject, dot_x);
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
    
    public bool ShouldReturn()
    {
        GameObject enemy = GetEnemy();
        if(!enemy) return false;

        returner.CheckReturn(enemy.transform.position);

        return returner.shouldReturn;
    }

    public bool IsAtSpawnpoint()
    {
        return returner.IsAtSpawnpoint(agentVel.stoppingRange);
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;
    
    public bool showArrivalRangeGizmo = true;
    public bool showAttackRangeGizmo = true;
    public bool showMaintainDistanceGizmo = true;

    public Color gizmoColor = new(1, 1, 1, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;

        if(showArrivalRangeGizmo)
        Gizmos.DrawWireSphere(transform.position, arrivalRange);

        if(showAttackRangeGizmo)
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if(showMaintainDistanceGizmo)
        Gizmos.DrawWireSphere(transform.position, maintainDistance);
    }
}
