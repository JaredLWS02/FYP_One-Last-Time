using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pilot))]
[RequireComponent(typeof(SideMove))]
[RequireComponent(typeof(SideTurn))]
//[RequireComponent(typeof(AISideSeek))]
//[RequireComponent(typeof(AISidePathseeker))]
//[RequireComponent(typeof(AISideWander))]
[RequireComponent(typeof(JumpScript))]
[RequireComponent(typeof(GroundCheck))]
[RequireComponent(typeof(Radar))]

public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;
    SideMove move;
    SideTurn turn;
    //AISideSeek seek;
    //AISidePathseeker pathseeker;
    //AISideWander wander;
    JumpScript jump;
    GroundCheck ground;
    Radar radar;

    void Awake()
    {
        pilot = GetComponent<Pilot>();
        move = GetComponent<SideMove>();
        turn = GetComponent<SideTurn>();
        // seek = GetComponent<AISideSeek>();
        // pathseeker = GetComponent<AISidePathseeker>();
        // wander = GetComponent<AISideWander>();
        jump = GetComponent<JumpScript>();
        ground = GetComponent<GroundCheck>();
        radar = GetComponent<Radar>();
    }

    void Start()
    {
        EventManager.Current.OnSpawn(gameObject);
    }

    // Actions ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;

    [Header("Toggles")]
    public bool AllowJump;

    // Event Manager ============================================================================

    void OnEnable()
    {
        EventManager.Current.TryMoveXEvent += OnTryMoveX;
        EventManager.Current.TryMoveYEvent += OnTryMoveY;
        EventManager.Current.TryJumpEvent += OnTryJump;
    }
    void OnDisable()
    {
        EventManager.Current.TryMoveXEvent -= OnTryMoveX;
        EventManager.Current.TryMoveYEvent -= OnTryMoveY;
        EventManager.Current.TryJumpEvent -= OnTryJump;
    }

    // Move ============================================================================

    void OnTryMoveX(GameObject mover, float input_x)
    {
        if(mover!=gameObject) return;

        if(!AllowMoveX) return;

        EventManager.Current.OnMoveX(gameObject, input_x);
    }

    void OnTryMoveY(GameObject mover, float input_y)
    {
        if(mover!=gameObject) return;

        if(!AllowMoveY) return;

        EventManager.Current.OnMoveY(gameObject, input_y);
    }

    // Jump ============================================================================
    
    void OnTryJump(GameObject jumper, float input)
    {
        if(jumper!=gameObject) return;

        if(!AllowJump) return;

        EventManager.Current.OnJump(gameObject, input);
    }

    public bool IsGrounded()
    {
        return ground.IsGrounded();
    }

    // ============================================================================

    public void SeekMove(Transform target)
    {
        // pathseeker.target = target;
        // pathseeker.Move();
    }

    // Wander ============================================================================
    
    public Transform GetWander()
    {
        return null; //wander.wanderTr;
    }

    public void SeekWander()
    {
        ChangeRange(huggyRange);
        SeekMove(GetWander());
    }

    // Radar ============================================================================

    [Header("Radar")]
    public string enemyTag = "Player";

    public GameObject GetClosest(string tag)
    {
        List<GameObject> targets = radar.GetTargetsWithTag(tag);
        return radar.GetClosest(targets);
    }

    public GameObject GetEnemy()
    {
        return GetClosest(enemyTag);
    }

    public void SeekEnemy()
    {
        GameObject enemy = GetEnemy();
        if(!enemy) return;

        ChangeRange(huggyRange);
        SeekMove(enemy.transform);
    }

    // Ranges ============================================================================

    [Header("Ranges")]
    public float huggyRange=1;
    public float attackRange=3;

    public void ChangeRange(float to)
    {
        //seek.stoppingRange = to;
    }

    public bool IsInRange(GameObject target, float range)
    {
        if(!target) return false;
        return Vector2.Distance(transform.position, target.transform.position) <= range;
    }

    public bool IsInAttackRange()
    {
        return IsInRange(GetEnemy(), attackRange);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, huggyRange);
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // ============================================================================

    bool IsFacingTarget(GameObject target)
    {
        if(!target) return false;

        return (target.transform.position.x >= transform.position.x && turn.faceR)
            || (target.transform.position.x < transform.position.x && !turn.faceR);
    }

    public void FaceEnemy()
    {
        GameObject enemy = GetEnemy();

        if(!enemy) return;

        //pathseeker.target = enemy.transform;
        
        if(IsFacingTarget(enemy)) return;

        float x_dir = turn.faceR ? -1 : 1;

        EventManager.Current.OnTryMoveX(gameObject, x_dir);
    }
}
