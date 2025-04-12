using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ForceReceiver : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.ForceReceivedEvent += OnForceReceived;
    }
    void OnDisable()
    {
        EventM.ForceReceivedEvent -= OnForceReceived;
    }
    
    // ============================================================================

    public float forceMult = 1;
    public bool stun = true;

    [Header("Optional")]
    public AnimSO customPullStunAnim;
    public AnimSO customPushStunAnim;

    void OnForceReceived(GameObject victim, GameObject attacker, float force, Vector3 direction, bool pull)
    {
        if(owner != victim) return;

        rb.AddForce(force * forceMult * direction, ForceMode.Impulse);

        TryStun(owner, attacker, pull);

        if(pull) events.OnPulled?.Invoke(direction);
        else events.OnPushed?.Invoke(direction);
    }

    void TryStun(GameObject victim, GameObject attacker, bool pull)
    {
        if(!stun) return;
        if(owner != victim) return;

        AnimSO stun_anim = pull ? customPullStunAnim : customPushStunAnim;

        EventM.OnStunAnim(owner, attacker, stun_anim, owner.transform.position);
    }

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent<Vector3> OnPulled;
        public UnityEvent<Vector3> OnPushed;
    }
    [Space]
    public Events events;
}
