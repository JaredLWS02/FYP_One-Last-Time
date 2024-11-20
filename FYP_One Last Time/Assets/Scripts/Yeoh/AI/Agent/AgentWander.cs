using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentWander : MonoBehaviour
{
    public GameObject owner;
    public NavMeshAgent agent;
    public AgentVehicle vehicle;

    // ============================================================================
    
    [Header("Wander")]
    public Transform wanderGoal;
    public RandomDoughnut wanderDoughnut;

    Vector3 startPos;
    Vector3 goalPos;
    
    void Awake()
    {
        wanderGoal.parent=null;

        startPos = owner.transform.position;
        goalPos = owner.transform.position;
    }

    void FixedUpdate()
    {
        wanderGoal.position = goalPos;
    }

    // ============================================================================

    [Header("Random Relocate")]
    public Vector2 relocateSeconds = new(1,4);
    public int maxRetries = 1000;

    public Vector3 axisMult = Vector3.one;

    void OnEnable()
    {
        StartCoroutine(Relocating());
    }

    IEnumerator Relocating()
    {
        while(true)
        {
            float t = Random.Range(relocateSeconds.x, relocateSeconds.y);
            yield return new WaitForSeconds(t);
            Relocate();
        }
    }

    void Relocate()
    {
        // ignore if main agent's goal is not the wander goal
        if(vehicle.goal != wanderGoal) return;

        if(IsTooFarFromStart())
        {
            goalPos = startPos;
            return;
        }
        
        for(int i=0; i<maxRetries; i++)
        {
            Vector3 random_spot = GetDoughnutAroundAgent();

            random_spot = SnapToNavMesh(random_spot);

            random_spot.Scale(axisMult); // same as multiply xyz

            if(IsTooNearDoughnut(random_spot)) continue;

            if(IsTooFarFromStart(random_spot)) continue;

            if(IsPathable(random_spot))
            {
                goalPos = random_spot;
                return;
            }
        }
        Debug.Log($"{owner.name}: Couldn't find place to wander to");
    }
    
    // ============================================================================

    Vector3 GetDoughnutAroundAgent()
    {
        return wanderDoughnut.GetRandomPos(owner.transform.position);
    }

    // ============================================================================

    Vector3 SnapToNavMesh(Vector3 pos)
    {
        if(NavMesh.SamplePosition(pos, out NavMeshHit hit, 9999, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return owner.transform.position;
    }

    // ============================================================================
    
    bool IsTooNearDoughnut(Vector3 pos)
    {
        float distance = Vector3.Distance(owner.transform.position, pos);
        return distance < wanderDoughnut.rangeMinMax.x;
    }

    // ============================================================================

    // zero/negative for infinite
    public float maxRangeFromStart = -1;

    bool IsTooFarFromStart(Vector3 pos)
    {
        if(maxRangeFromStart <= 0) return false;

        float distance = Vector3.Distance(startPos, pos);
        return distance > maxRangeFromStart;
    }

    bool IsTooFarFromStart()
    {
        return IsTooFarFromStart(owner.transform.position);
    }

    // ============================================================================

    bool IsPathable(Vector3 pos)
    {
        NavMeshPath path = new();
        agent.CalculatePath(pos, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    // ============================================================================
    
    [Header("Agent")]
    public float wanderArrivalRange=1;

    public void SetGoalToWander()
    {
        vehicle.SetRange(wanderArrivalRange);
        vehicle.SetGoal(wanderGoal);
    }

    // ============================================================================

    void OnDestroy()
    {
        Destroy(wanderGoal.gameObject);
    }
    
    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;
    public Color gizmoColor = new(1, 1, 1, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        if(maxRangeFromStart<=0) return;

        Vector3 pos = Application.isPlaying ? startPos : owner.transform.position;
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(pos, maxRangeFromStart);
        Gizmos.DrawWireSphere(owner.transform.position, wanderArrivalRange);
    }
}
