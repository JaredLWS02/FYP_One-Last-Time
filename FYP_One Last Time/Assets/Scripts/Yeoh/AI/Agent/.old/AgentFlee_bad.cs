// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// [RequireComponent(typeof(NavMeshAgent))]

// public class AgentFlee : MonoBehaviour
// {
//     NavMeshAgent agent;

//     public Transform threat;
//     public RandomDoughnut threatDoughnut;

//     Vector3 goalPos;
    
//     public Transform goal;

//     void Awake()
//     {
//         agent = GetComponent<NavMeshAgent>();

//         goalPos = agent.transform.position;
//     }

//     void OnEnable()
//     {
//         StartCoroutine(Relocating());
//     }    

//     void FixedUpdate()
//     {
//         goal.position = goalPos;
//     }

//     // ============================================================================

//     [Header("Flee Relocate")]
//     public Vector2 relocateSeconds = new(1,2);
//     public int maxRetries = 1000;

//     public Vector3 axisMult = Vector3.one;

//     IEnumerator Relocating()
//     {
//         while(true)
//         {
//             float t = Random.Range(relocateSeconds.x, relocateSeconds.y);
//             yield return new WaitForSeconds(t);
//             Relocate();
//         }
//     }

//     void Relocate()
//     {
//         for(int i=0; i<maxRetries; i++)
//         {
//             Vector3 random_flee_spot = GetDoughnutAroundThreat();

//             random_flee_spot = SnapToNavMesh(random_flee_spot);

//             random_flee_spot.Scale(axisMult); // same as multiply xyz

//             // must be opposite direction of threat
//             if(GetFleeDotProduct(random_flee_spot) >= 0) continue;            

//             // must be further than threat
//             if(!IsFurtherThanThreat(random_flee_spot)) continue;

//             // not too near the threat
//             if(IsTooNearDoughnut(random_flee_spot)) continue;

//             if(IsPathable(random_flee_spot))
//             {
//                 goalPos = random_flee_spot;
//                 return;
//             }
//         }
//         Debug.Log($"{agent.name}: Couldn't find place to flee to");
//     }
    
//     // ============================================================================

//     Vector3 GetDoughnutAroundThreat()
//     {
//         return threatDoughnut.GetRandomPos(threat.position);
//     }

//     // ============================================================================

//     Vector3 SnapToNavMesh(Vector3 pos)
//     {
//         if(NavMesh.SamplePosition(pos, out NavMeshHit hit, 9999, NavMesh.AllAreas))
//         {
//             return hit.position;
//         }
//         return agent.transform.position;
//     }

//     // ============================================================================

//     float GetFleeDotProduct(Vector3 flee_pos)
//     {
//         Vector3 agent_to_fleepos = (flee_pos - agent.transform.position).normalized;
        
//         Vector3 agent_to_threat = (threat.position - agent.transform.position).normalized;

//         return Vector3.Dot(agent_to_fleepos, agent_to_threat);
//     }

//     // ============================================================================

//     bool IsFurtherThanThreat(Vector3 flee_pos)
//     {
//         float agent_to_threat = Vector3.Distance(agent.transform.position, threat.position);

//         float flee_to_threat = Vector3.Distance(flee_pos, threat.position);

//         return flee_to_threat > agent_to_threat;
//     }

//     // ============================================================================
    
//     bool IsTooNearDoughnut(Vector3 pos)
//     {
//         float distance = Vector3.Distance(threat.position, pos);
//         return distance < threatDoughnut.rangeMinMax.x;
//     }

//     // ============================================================================

//     bool IsPathable(Vector3 pos)
//     {
//         NavMeshPath path = new();
//         agent.CalculatePath(pos, path);
//         return path.status == NavMeshPathStatus.PathComplete;
//     }
// }
