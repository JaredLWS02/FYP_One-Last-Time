using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSequence : MonoBehaviour
{
    [System.Serializable]
    public class DelayedEvent
    {
        public UnityEvent uEvent;
        public float delay;
    }

    public List<DelayedEvent> delayedEvents = new();

    // ============================================================================

    public bool canInterrupt=true;

    public void PlaySequence()
    {
        if(PlayingSequence_crt!=null)
        {
            if(canInterrupt)
            {
                StopCoroutine(PlayingSequence_crt);
                PlayingSequence_crt=null;
            }
            else return;
        }

        PlayingSequence_crt = StartCoroutine(PlayingSequence());
    }

    Coroutine PlayingSequence_crt;

    IEnumerator PlayingSequence()
    {
        for(int i=0; i < delayedEvents.Count; i++)
        {
            yield return new WaitForSeconds(delayedEvents[i].delay);
            delayedEvents[i].uEvent?.Invoke();
        }

        PlayingSequence_crt=null;
    }

    // ============================================================================

    void OnDisable()
    {
        PlayingSequence_crt=null;
    }
}
