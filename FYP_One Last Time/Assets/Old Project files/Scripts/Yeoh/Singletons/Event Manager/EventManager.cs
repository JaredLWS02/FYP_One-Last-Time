using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Current;

    void Awake()
    {
        if(Current!=null && Current!=this)
        {
            Destroy(gameObject);
            return;
        }
        Current = this;
    }

    // ==================================================================================================================

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(Current!=this) Destroy(gameObject);
    }
    

    // Actors ==================================================================================================================

    public event Action<GameObject> SpawnEvent;

    public void OnSpawn(GameObject spawned)
    {
        SpawnEvent?.Invoke(spawned);
    }


    // Control ==================================================================================================================

    public event Action<GameObject, Pilot.Type> SwitchPilotEvent;

    public void OnSwitchPilot(GameObject who, Pilot.Type to)
    {
        SwitchPilotEvent?.Invoke(who, to);
    }   


    // Actions ==================================================================================================================

    public event Action<GameObject, float> TryMoveXEvent;
    public event Action<GameObject, float> MoveXEvent;

    public void OnTryMoveX(GameObject mover, float input_x)
    {
        TryMoveXEvent?.Invoke(mover, input_x);
    }  
    public void OnMoveX(GameObject mover, float input_x)
    {
        MoveXEvent?.Invoke(mover, input_x);
    } 

    public event Action<GameObject, float> TryMoveYEvent;
    public event Action<GameObject, float> MoveYEvent;

    public void OnTryMoveY(GameObject mover, float input_y)
    {
        TryMoveYEvent?.Invoke(mover, input_y);
    } 
    public void OnMoveY(GameObject mover, float input_y)
    {
        MoveYEvent?.Invoke(mover, input_y);
    }

    public event Action<GameObject, float> TryJumpEvent;
    public event Action<GameObject, float> JumpEvent;

    public void OnTryJump(GameObject jumper, float input)
    {
        TryJumpEvent?.Invoke(jumper, input);
    }
    public void OnJump(GameObject jumper, float input)
    {
        JumpEvent?.Invoke(jumper, input);
    }    
    
    
    // Combat ==================================================================================================================

    public event Action<GameObject> AttackEvent;
    public event Action<GameObject, GameObject, HurtInfo> HitEvent; // ignores iframe
    public event Action<GameObject, GameObject, HurtInfo> HurtEvent; // respects iframe
    public event Action<GameObject, GameObject, HurtInfo> DeathEvent;
   
    public void OnAttack(GameObject attacker)
    {
        AttackEvent?.Invoke(attacker);
    }    
    public void OnHit(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        HitEvent?.Invoke(attacker, victim, hurtInfo);
    }    
    public void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        HurtEvent?.Invoke(victim, attacker, hurtInfo);
    }
    public void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        DeathEvent?.Invoke(victim, killer, hurtInfo);
    }

    
    // Item ==================================================================================================================
    
    public event Action<GameObject, GameObject, LootInfo> LootEvent;

    public void OnLoot(GameObject looter, GameObject loot, LootInfo lootInfo)
    {
        LootEvent?.Invoke(looter, loot, lootInfo);
    }


    // Ability ==================================================================================================================
    
    public event Action<GameObject, string> TryStartCastEvent;
    public event Action<GameObject, string> StartCastEvent;
    
    public void OnTryStartCast(GameObject caster, string ability_name)
    {
        TryStartCastEvent?.Invoke(caster, ability_name);
    }
    public void OnStartCast(GameObject caster, string ability_name)
    {
        StartCastEvent?.Invoke(caster, ability_name);
    }

    public event Action<GameObject, AbilitySlot> CastingEvent;
    public event Action<GameObject, AbilitySlot> CastWindUpEvent;
    public event Action<GameObject, AbilitySlot> CastReleaseEvent;
    public event Action<GameObject> CastFinishEvent;
    public event Action<GameObject> CastCancelEvent;

    public void OnCasting(GameObject caster, AbilitySlot abilitySlot)
    {
        CastingEvent?.Invoke(caster, abilitySlot);
    }
    public void OnCastWindUp(GameObject caster, AbilitySlot abilitySlot)
    {
        CastWindUpEvent?.Invoke(caster, abilitySlot);
    }
    public void OnCastRelease(GameObject caster, AbilitySlot abilitySlot)
    {
        CastReleaseEvent?.Invoke(caster, abilitySlot);
    }
    public void OnCastFinish(GameObject caster)
    {
        CastFinishEvent?.Invoke(caster);
    }
    public void OnCastCancel(GameObject caster)
    {
        CastCancelEvent?.Invoke(caster);
    }


    // UI ==================================================================================================================

    public event Action<GameObject, float, float> UIBarUpdateEvent;

    public void OnUIBarUpdate(GameObject owner, float value, float valueMax)
    {
        UIBarUpdateEvent?.Invoke(owner, value, valueMax);
    }


    // Mouse/Touch ==================================================================================================================

    public event Action<Vector3> Click2DEvent;
    public event Action<Vector3, float, Vector3, Vector3, Vector3> Swipe2DEvent;
    public event Action<GameObject> ClickObjectEvent;
    
    public void OnClick2D(Vector3 pos)
    {
        Click2DEvent?.Invoke(pos);
    }
    public void OnSwipe2D(Vector3 startPos, float magnitude, Vector3 direction, Vector3 endPos)
    {
        Vector3 midPos = Vector3.Lerp(startPos, endPos, .5f);

        Swipe2DEvent?.Invoke(startPos, magnitude, direction, endPos, midPos);
    }
    public void OnClickObject(GameObject clicked)
    {
        ClickObjectEvent?.Invoke(clicked);
    }
    

    // Sound ==================================================================================================================

    public event Action<GameObject> IdleVoiceEvent;

    public void OnIdleVoice(GameObject subject)
    {
        IdleVoiceEvent?.Invoke(subject);
    }

    
    // Old ==================================================================================================================

    public event Action<GameObject> TrySwitchEvent;
    
    public void OnTrySwitch(GameObject switcher)
    {
        TrySwitchEvent?.Invoke(switcher);
    }  
}