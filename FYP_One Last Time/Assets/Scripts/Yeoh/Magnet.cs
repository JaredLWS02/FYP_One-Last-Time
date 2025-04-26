using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magnet : MonoBehaviour
{
    public Transform owner;
    public Radar radar;

    public float magnetDelay=1;
    bool canMagnet;

    public float moveTime=.5f;
    public Vector3 destinationOffset = new(0, 1.5f ,0);

    // ============================================================================

    void OnEnable()
    {
        if(magnetDelay>0) StartCoroutine(MagnetDelaying());
    }

    IEnumerator MagnetDelaying()
    {
        canMagnet=false;
        yield return new WaitForSeconds(magnetDelay);
        canMagnet=true;
    }

    // ============================================================================

    void FixedUpdate()
    {
        var target = radar.GetClosestTarget();
        if(!target) return;
        if(!canMagnet) return;

        var offset = target.transform.TransformDirection(destinationOffset);

        var destination = target.transform.position + offset;

        SmoothTowards(owner.position, destination, moveTime);

        events.OnMagnetising?.Invoke(destination);
    }

    // ============================================================================

    Vector3 velocity;

    void SmoothTowards(Vector3 from, Vector3 to, float move_time)
    {
        owner.position = Vector3.SmoothDamp(from, to, ref velocity, move_time);
    }

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent<Vector3> OnMagnetising;
    }
    [Space]
    public Events events;
}
