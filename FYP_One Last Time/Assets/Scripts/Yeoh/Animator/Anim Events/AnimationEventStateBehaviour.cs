using UnityEngine;
using UnityEngine.Events;

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    public string eventName;

    [Range(0f, .9f)]
    public float triggerTime;
    
    public bool triggerOnExit=true;

    bool hasTriggered;

    AnimationEventReceiver receiver;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasTriggered=false;

        receiver = animator.GetComponent<AnimationEventReceiver>();
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(hasTriggered) return;

        float currentTime = stateInfo.normalizedTime % 1f;

        if(currentTime >= triggerTime)
        {
            TriggerEvent(animator);
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(hasTriggered) return;

        if(!triggerOnExit) return;

        TriggerEvent(animator);
    }


    void TriggerEvent(Animator animator)
    {
        hasTriggered=true;
        NotifyReceiver(animator);
    }


    void NotifyReceiver(Animator animator)
    {
        if(receiver==null) return;

        receiver.OnAnimationEventTriggered(eventName);
    }
}
