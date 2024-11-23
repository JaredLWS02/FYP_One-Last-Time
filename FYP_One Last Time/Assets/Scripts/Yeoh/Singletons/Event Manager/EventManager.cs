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
    
    // Spawn ==================================================================================================================

    public event Action<GameObject> SpawnedEvent;

    public void OnSpawned(GameObject spawned)
    {
        SpawnedEvent?.Invoke(spawned);
    }
    
    // Controls ==================================================================================================================

    public event Action<GameObject, PilotType> SwitchPilotEvent;

    public void OnSwitchPilot(GameObject who, PilotType to)
    {
        SwitchPilotEvent?.Invoke(who, to);
    }
    
    // Agent Controls ==================================================================================================================

    public event Action<GameObject, Vector2> AgentTryMoveEvent;
    public event Action<GameObject, float> AgentTryFlipEvent;
    public event Action<GameObject> AgentTryJumpEvent;
    public event Action<GameObject> AgentTryJumpCutEvent;
    public event Action<GameObject, Vector3> AgentTryAutoJumpEvent;
    public event Action<GameObject, string> AgentTryAttackEvent;

    public void OnAgentTryMove(GameObject who, Vector2 input)
    {
        AgentTryMoveEvent?.Invoke(who, input);
    }
    public void OnAgentTryFlip(GameObject who, float dir_x)
    {
        AgentTryFlipEvent?.Invoke(who, dir_x);
    }
    public void OnAgentTryJump(GameObject who)
    {
        AgentTryJumpEvent?.Invoke(who);
    }
    public void OnAgentTryJumpCut(GameObject who)
    {
        AgentTryJumpCutEvent?.Invoke(who);
    }
    public void OnAgentTryAutoJump(GameObject who, Vector3 dir)
    {
        AgentTryAutoJumpEvent?.Invoke(who, dir);
    }
    public void OnAgentTryAttack(GameObject who, string type)
    {
        AgentTryAttackEvent?.Invoke(who, type);
    }
    
    // Movement ==================================================================================================================

    public event Action<GameObject, Vector2> TryMoveEvent;
    public event Action<GameObject, Vector2> MoveEvent;

    public void OnTryMove(GameObject mover, Vector2 input)
    {
        TryMoveEvent?.Invoke(mover, input);
    }  
    public void OnMove(GameObject mover, Vector2 input)
    {
        MoveEvent?.Invoke(mover, input);
    } 

    public event Action<GameObject, float> TryFlipEvent;
    public event Action<GameObject, float> FlipEvent;
    public event Action<GameObject> CancelFlipDelayEvent;

    public void OnTryFlip(GameObject who, float input_x)
    {
        TryFlipEvent?.Invoke(who, input_x);
    }  
    public void OnFlip(GameObject who, float input_x)
    {
        FlipEvent?.Invoke(who, input_x);
    } 
    public void OnCancelFlipDelay(GameObject who)
    {
        CancelFlipDelayEvent?.Invoke(who);
    } 
    
    // Jumps ==================================================================================================================
    
    public event Action<GameObject> TryJumpEvent;
    public event Action<GameObject> JumpEvent;
    public event Action<GameObject> JumpedEvent;

    public void OnTryJump(GameObject jumper)
    {
        TryJumpEvent?.Invoke(jumper);
    }
    public void OnJump(GameObject jumper)
    {
        JumpEvent?.Invoke(jumper);
    }
    public void OnJumped(GameObject jumper)
    {
        JumpedEvent?.Invoke(jumper);
    }    

    public event Action<GameObject> TryJumpCutEvent;
    public event Action<GameObject> JumpCutEvent;
    public event Action<GameObject> JumpCuttedEvent;

    public void OnTryJumpCut(GameObject jumper)
    {
        TryJumpCutEvent?.Invoke(jumper);
    }
    public void OnJumpCut(GameObject jumper)
    {
        JumpCutEvent?.Invoke(jumper);
    }
    public void OnJumpCutted(GameObject jumper)
    {
        JumpCuttedEvent?.Invoke(jumper);
    }    

    public event Action<GameObject, Vector3> TryAutoJumpEvent;
    public event Action<GameObject, Vector3> AutoJumpEvent;
    public event Action<GameObject, Vector3> AutoJumpingEvent;
    public event Action<GameObject, Vector3> AutoJumpedEvent;
    public event Action<GameObject> CancelAutoJumpEvent;
    public event Action<GameObject> AutoJumpCancelledEvent;

    public void OnTryAutoJump(GameObject jumper, Vector3 jump_dir)
    {
        TryAutoJumpEvent?.Invoke(jumper, jump_dir);
    }    
    public void OnAutoJump(GameObject jumper, Vector3 jump_dir)
    {
        AutoJumpEvent?.Invoke(jumper, jump_dir);
    }    
    public void OnAutoJumping(GameObject jumper, Vector3 jump_dir)
    {
        AutoJumpingEvent?.Invoke(jumper, jump_dir);
    }    
    public void OnAutoJumped(GameObject jumper, Vector3 jump_dir)
    {
        AutoJumpedEvent?.Invoke(jumper, jump_dir);
    }
    public void OnCancelAutoJump(GameObject jumper)
    {
        CancelAutoJumpEvent?.Invoke(jumper);
    }    
    public void OnAutoJumpCancelled(GameObject jumper)
    {
        AutoJumpCancelledEvent?.Invoke(jumper);
    }    
    
    public event Action<GameObject> FastFallStartEvent;
    public event Action<GameObject> FastFallEndEvent;

    public void OnFastFallStart(GameObject who)
    {
        FastFallStartEvent?.Invoke(who);
    }
    public void OnFastFallEnd(GameObject who)
    {
        FastFallEndEvent?.Invoke(who);
    }    

    public event Action<GameObject> LandGroundEvent;
    public event Action<GameObject> LeaveGroundEvent;

    public void OnLandGround(GameObject who)
    {
        LandGroundEvent?.Invoke(who);
    }
    public void OnLeaveGround(GameObject who)
    {
        LeaveGroundEvent?.Invoke(who);
    }    

    // One Way Platform ==================================================================================================================

    public event Action<GameObject, Collider, bool> DeplatformEvent;

    public void OnDeplatform(GameObject who, Collider platform, bool toggle)
    {
        DeplatformEvent?.Invoke(who, platform, toggle);
    }

    // Dash ==================================================================================================================
    
    public event Action<GameObject> TryDashEvent;
    public event Action<GameObject> DashEvent;
    public event Action<GameObject> DashedEvent;
    public event Action<GameObject> CancelDashEvent;
    public event Action<GameObject> DashCancelledEvent;

    public void OnTryDash(GameObject who)
    {
        TryDashEvent?.Invoke(who);
    }
    public void OnDash(GameObject who)
    {
        DashEvent?.Invoke(who);
    }    
    public void OnDashed(GameObject who)
    {
        DashedEvent?.Invoke(who);
    }    
    public void OnCancelDash(GameObject who)
    {
        CancelDashEvent?.Invoke(who);
    }    
    public void OnDashCancelled(GameObject who)
    {
        DashCancelledEvent?.Invoke(who);
    }

    // Vine Pull ==================================================================================================================

    //public event Action<GameObject> TryVinePullEvent;
    //public event Action<GameObject> VinePullEvent;
    //public event Action<GameObject> VinePulledEvent;
    //public event Action<GameObject> VinePullCutEvent;


    // Attacks and Combos ==================================================================================================================

    public event Action<GameObject, string> TryComboEvent;
    public event Action<GameObject, string> ComboEvent;
    public event Action<GameObject, AttackSO> AttackedEvent;
    public event Action<GameObject, AttackSO> AttackWindedUpEvent;
    public event Action<GameObject, AttackSO> AttackReleasedEvent;
    public event Action<GameObject> CancelAttackEvent;
    public event Action<GameObject> AttackCancelledEvent;

    public void OnTryCombo(GameObject attacker, string combo_name)
    {
        TryComboEvent?.Invoke(attacker, combo_name);
    }
    public void OnCombo(GameObject attacker, string combo_name)
    {
        ComboEvent?.Invoke(attacker, combo_name);
    }
    public void OnAttacked(GameObject attacker, AttackSO attackSO)
    {
        AttackedEvent?.Invoke(attacker, attackSO);
    }  
    public void OnAttackWindedUp(GameObject attacker, AttackSO attackSO)
    {
        AttackWindedUpEvent?.Invoke(attacker, attackSO);
    }  
    public void OnAttackReleased(GameObject attacker, AttackSO attackSO)
    {
        AttackReleasedEvent?.Invoke(attacker, attackSO);
    }  
    public void OnCancelAttack(GameObject attacker)
    {
        CancelAttackEvent?.Invoke(attacker);
    }
    public void OnAttackCancelled(GameObject attacker)
    {
        AttackCancelledEvent?.Invoke(attacker);
    }
    
    // Parry and Riposte ==================================================================================================================

    public event Action<GameObject> TryRaiseParryEvent;
    public event Action<GameObject> RaiseParryEvent;
    public event Action<GameObject> RaisedParryEvent;
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> TryParryEvent;
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> ParryEvent;
    public event Action<GameObject, string> TryRiposteComboEvent;
    public event Action<GameObject> CancelParryEvent;
    public event Action<GameObject> ParryCancelledEvent;

    public void OnTryRaiseParry(GameObject defender)
    {
        TryRaiseParryEvent?.Invoke(defender);
    }
    public void OnRaiseParry(GameObject defender)
    {
        RaiseParryEvent?.Invoke(defender);
    }
    public void OnRaisedParry(GameObject defender)
    {
        RaisedParryEvent?.Invoke(defender);
    }
    public void OnTryParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        TryParryEvent?.Invoke(defender, attacker, hurtbox, contactPoint);
    }
    public void OnParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        ParryEvent?.Invoke(defender, attacker, hurtbox, contactPoint);
    }
    public void OnTryRiposteCombo(GameObject attacker, string combo_name)
    {
        TryRiposteComboEvent?.Invoke(attacker, combo_name);
    }
    public void OnCancelParry(GameObject defender)
    {
        CancelParryEvent?.Invoke(defender);
    }
    public void OnParryCancelled(GameObject defender)
    {
        ParryCancelledEvent?.Invoke(defender);
    }
    
    // Ability ==================================================================================================================

    public event Action<GameObject, string> TryAbilityEvent;
    public event Action<GameObject, string> AbilityEvent;
    public event Action<GameObject, AbilitySO> CastingEvent;
    public event Action<GameObject, AbilitySlot> CastEvent;
    public event Action<GameObject, AbilitySO> CastReleasedEvent;
    public event Action<GameObject> CancelCastEvent;
    public event Action<GameObject> CastCancelledEvent;
    
    public void OnTryAbility(GameObject caster, string ability_name)
    {
        TryAbilityEvent?.Invoke(caster, ability_name);
    }
    public void OnAbility(GameObject caster, string ability_name)
    {
        AbilityEvent?.Invoke(caster, ability_name);
    }
    public void OnCasting(GameObject caster, AbilitySO abilitySO)
    {
        CastingEvent?.Invoke(caster, abilitySO);
    }
    public void OnCast(GameObject caster, AbilitySlot slot)
    {
        CastEvent?.Invoke(caster, slot);
    }
    public void OnCastReleased(GameObject caster, AbilitySO abilitySO)
    {
        CastReleasedEvent?.Invoke(caster, abilitySO);
    }
    public void OnCancelCast(GameObject caster)
    {
        CancelCastEvent?.Invoke(caster);
    }
    public void OnCastCancelled(GameObject caster)
    {
        CastCancelledEvent?.Invoke(caster);
    }  
    
    // Hurt ==================================================================================================================
    
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> TryHurtEvent; // ignores iframe/block/parry
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> HurtEvent;
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> HurtedEvent; // respects iframe/block/parry
    public event Action<GameObject, GameObject, float> HealEvent;
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> PoiseBreakEvent;
    public event Action<GameObject, float, Vector3> TryKnockbackEvent;
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> DeathEvent;

    public void OnTryHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        TryHurtEvent?.Invoke(victim, attacker, hurtbox, contactPoint);
    }    
    public void OnHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        HurtEvent?.Invoke(victim, attacker, hurtbox, contactPoint);
    }
    public void OnHurted(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        HurtedEvent?.Invoke(victim, attacker, hurtbox, contactPoint);
    }
    public void OnHeal(GameObject who, GameObject healer, float amount)
    {
        HealEvent?.Invoke(who, healer, amount);
    }
    public void OnPoiseBroke(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        PoiseBreakEvent?.Invoke(victim, attacker, hurtbox, contactPoint);
    }
    public void OnTryKnockback(GameObject who, float force, Vector3 contactPoint)
    {
        TryKnockbackEvent?.Invoke(who, force, contactPoint);
    }
    public void OnDeath(GameObject victim, GameObject killer, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        DeathEvent?.Invoke(victim, killer, hurtbox, contactPoint);
    }

    // Stun ==================================================================================================================

    public event Action<GameObject, GameObject, HurtboxSO, Vector3> TryStunEvent;
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> StunEvent;
    public event Action<GameObject, GameObject, HurtboxSO, Vector3> StunnedEvent;
    public event Action<GameObject> CancelStunEvent;
    
    public void OnTryStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        TryStunEvent?.Invoke(victim, attacker, hurtbox, contactPoint);
    }
    public void OnStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        StunEvent?.Invoke(victim, attacker, hurtbox, contactPoint);
    }
    public void OnStunned(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        StunnedEvent?.Invoke(victim, attacker, hurtbox, contactPoint);
    }
    public void OnCancelStun(GameObject who)
    {
        CancelStunEvent?.Invoke(who);
    } 

    // iframe ==================================================================================================================

    public event Action<GameObject, float> TryIFrameEvent;
    public event Action<GameObject, bool> IFrameToggleEvent;

    public void OnTryIFrame(GameObject who, float duration)
    {
        TryIFrameEvent?.Invoke(who, duration);
    }
    public void OnIFrameToggle(GameObject who, bool toggle)
    {
        IFrameToggleEvent?.Invoke(who, toggle);
    } 

    // Animator ==================================================================================================================

    public event Action<GameObject, string, int, float> PlayAnimEvent;

    public void OnPlayAnim(GameObject who, string animName, int animLayer, float blendTime=0)
    {
        PlayAnimEvent?.Invoke(who, animName, animLayer, blendTime);
    }

    // Boss ==================================================================================================================

    public event Action<GameObject> ActionCompleteEvent;
    public event Action<GameObject> TryChangePhaseEvent;
    public event Action<GameObject, string> PhaseChangedEvent;

    public void OnActionComplete(GameObject who)
    {
        ActionCompleteEvent?.Invoke(who);
    }
    public void OnTryChangePhase(GameObject who)
    {
        TryChangePhaseEvent?.Invoke(who);
    }
    public void OnPhaseChanged(GameObject who, string phaseName)
    {
        PhaseChangedEvent?.Invoke(who, phaseName);
    }
    
    // UI ==================================================================================================================

    public event Action<GameObject, float, float> UIBarUpdateEvent;

    public void OnUIBarUpdate(GameObject owner, float value, float maxValue)
    {
        UIBarUpdateEvent?.Invoke(owner, value, maxValue);
    }

    // Inventory ==================================================================================================================
    
    public event Action<GameObject, GameObject, LootInfo> LootEvent;

    public void OnLoot(GameObject looter, GameObject loot, LootInfo lootInfo)
    {
        LootEvent?.Invoke(looter, loot, lootInfo);
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