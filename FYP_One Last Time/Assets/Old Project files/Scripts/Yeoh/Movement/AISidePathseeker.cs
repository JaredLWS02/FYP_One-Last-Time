using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AISideSeek))]

public class AISidePathseeker : MonoBehaviour
{
    Seeker seeker;
    AISideSeek aiMove;

    void Awake()
    {
        seeker = GetComponent<Seeker>();
        aiMove = GetComponent<AISideSeek>();
    }

    // ============================================================================
    
    Path path=null;

    [Header("Path")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0, .5f, 0);
    public Vector3 selfOffset = new Vector3(0, .5f, 0);

    // Update Path ============================================================================

    public float UpdatePathInterval=.25f;

    void OnEnable()
    {
        StartCoroutine(MakingPath());
    }

    IEnumerator MakingPath()
    {
        while(true)
        {
            yield return new WaitForSeconds(UpdatePathInterval);
            
            if(target)
            TryMakePath(transform.position + selfOffset, target.position + targetOffset);
        }
    }

    void TryMakePath(Vector3 from, Vector3 to)
    {
        if(!seeker.IsDone()) return;

        seeker.StartPath(from, to, OnPathCreated);
    }

    void OnPathCreated(Path new_path)
    {
        if(new_path.error) return;

        // new_path.vectorPath = GetNodesOutsideStoppingRange(new_path.vectorPath);
        // a better way is to give both the node pos and goal pos to ai seek)
        // arrival should only compare the distance between the goal pos, not the node pos

        if(new_path.vectorPath.Count==0) return;

        path = new_path;
        currentNodeIndex = Mathf.Min(startingNodeIndex, path.vectorPath.Count-1);
    }

    List<Vector3> GetNodesOutsideStoppingRange(List<Vector3> path_nodes)
    {
        List<Vector3> new_nodes = new();

        // add all the nodes that are outside stopping range
        foreach(Vector3 node in path_nodes)
        {
            float distance = Vector3.Distance(node, target.position + targetOffset);

            if(distance >= aiMove.stoppingRange)
            {
                new_nodes.Add(node);
            }
        }

        return new_nodes;
    }

    // Moving ============================================================================

    [Header("Move")]
    public int startingNodeIndex=2;
    int currentNodeIndex;
    //public float nextNodeRange=1;

    public void Move()
    {
        if(!target) return;
        if(path==null) return;
        if(path.vectorPath.Count==0) return;

        Vector3 targetNode = path.vectorPath[currentNodeIndex];
        TryContinue(targetNode);

        //aiMove.arrival = HasReachedEnd();
        aiMove.goal = target;
        aiMove.seekPos = targetNode;
        aiMove.Move();

        CheckNodeHeight(targetNode);
    }

    void TryContinue(Vector3 targetNode)
    {
        if(HasReachedEnd()) return;

        float distance = Vector3.Distance(transform.position + selfOffset, targetNode);

        float nextNodeRange = aiMove.stoppingRange;

        if(distance <= nextNodeRange)
        {
            currentNodeIndex++;
        }
    }

    bool HasReachedEnd()
    {
        if(path==null) return true;

        return currentNodeIndex >= path.vectorPath.Count-1;
    }

    // For Jump or Descend ============================================================================

    void CheckNodeHeight(Vector3 targetNode)
    {
        Vector3 selfPos = transform.position + selfOffset;
        float node_height = targetNode.y - selfPos.y;
        float nextNodeRange = aiMove.stoppingRange*.8f;

        // node is above
        if(node_height > nextNodeRange)
        {
            EventManager.Current.OnTryJump(gameObject, 1); // jump duh
            EventManager.Current.OnTryMoveY(gameObject, 1); // press up
        }
        // node is below
        else if(node_height < -nextNodeRange)
        {
            EventManager.Current.OnTryJump(gameObject, 0); // jumpcut
            EventManager.Current.OnTryMoveY(gameObject, -1); // press down
        }
    }
}
