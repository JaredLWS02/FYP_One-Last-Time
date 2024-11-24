using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IFrame : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryIFrameEvent += OnTryIFrame;
        EventM.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventM.TryIFrameEvent -= OnTryIFrame;
        EventM.DeathEvent -= OnDeath;
    }

    // ============================================================================

    public float seconds=.5f;
    public bool isActive {get; private set;}

    void OnTryIFrame(GameObject who, float duration)
    {
        if(who!=owner) return;

        if(duration<=0) return;

        if(iframing_crt!=null) StopCoroutine(iframing_crt);
        iframing_crt = StartCoroutine(IFraming(duration));
    }

    Coroutine iframing_crt;
    
    IEnumerator IFraming(float t)
    {
        isActive=true;

        EventM.OnIFrameToggle(owner, true);
        iframeEvents.OnIFrameToggle?.Invoke(true);

        yield return new WaitForSeconds(t);

        isActive=false;

        EventM.OnIFrameToggle(owner, false);
        iframeEvents.OnIFrameToggle?.Invoke(false);
    }

    // ============================================================================

    [Header("Die")]
    public bool iframeOnDeath=true;

    void OnDeath(GameObject victim, GameObject killer, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        isActive = iframeOnDeath;
    }

    // ============================================================================

    [System.Serializable]
    public struct IFrameEvents
    {
        public UnityEvent<bool> OnIFrameToggle;
    }
    [Space]
    public IFrameEvents iframeEvents;
}
