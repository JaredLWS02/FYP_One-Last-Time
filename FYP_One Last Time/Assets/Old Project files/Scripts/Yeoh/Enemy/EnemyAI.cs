using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pilot))]
[RequireComponent(typeof(SideMove))]
[RequireComponent(typeof(AISideSeek))]
[RequireComponent(typeof(AISidePathseeker))]
[RequireComponent(typeof(AISideWander))]
[RequireComponent(typeof(Jump2D))]
[RequireComponent(typeof(Radar2D))]

public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;
    SideMove move;
    AISideSeek seek;
    AISidePathseeker pathseeker;
    AISideWander wander;
    Radar2D radar;
    Jump2D jump;

    void Awake()
    {
        pilot = GetComponent<Pilot>();
        move = GetComponent<SideMove>();
        seek = GetComponent<AISideSeek>();
        pathseeker = GetComponent<AISidePathseeker>();
        wander = GetComponent<AISideWander>();
        radar = GetComponent<Radar2D>();
        jump = GetComponent<Jump2D>();
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
        return jump.IsGrounded();
    }

    // ============================================================================

    public void SeekMove(Transform target)
    {
        pathseeker.target = target;
        pathseeker.Move();
    }

    // Wander ============================================================================
    
    public Transform GetWander()
    {
        return wander.wanderTr;
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
        seek.stoppingRange = to;
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

        return (target.transform.position.x >= transform.position.x && move.faceR)
            || (target.transform.position.x < transform.position.x && !move.faceR);
    }

    public void FaceEnemy()
    {
        GameObject enemy = GetEnemy();

        if(!enemy) return;

        pathseeker.target = enemy.transform;
        
        if(IsFacingTarget(enemy)) return;

        float x_dir = move.faceR ? -1 : 1;

        EventManager.Current.OnTryMoveX(gameObject, x_dir);
    }
}
