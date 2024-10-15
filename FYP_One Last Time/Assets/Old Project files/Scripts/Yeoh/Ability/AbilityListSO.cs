using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilitySlot
{
    public AbilitySO ability;
    public float cooldownLeft=0;

    public bool IsEmpty() => ability==null;

    public bool IsCooling() => cooldownLeft>0;

    public void ResetCooldown() => cooldownLeft=0;

    public void DoCooldown() => cooldownLeft=ability.cooldown;

    public void UpdateCooldown()
    {
        if(IsCooling())
        {
            cooldownLeft -= Time.deltaTime;

            if(cooldownLeft<0)
            cooldownLeft=0;
        }
    }
};

// ============================================================================

[CreateAssetMenu]
public class AbilityListSO : ScriptableObject
{
    public List<AbilitySlot> abilitySlots;

    // Getters ============================================================================

    public AbilitySlot GetAbility(AbilitySO ability)
    {
        foreach(var slot in abilitySlots)
        {
            if(slot.ability == ability)
            {
                return slot;
            }
        }
        return null;
    }

    public AbilitySlot GetAbility(AbilitySlot ability)
    {
        foreach(var slot in abilitySlots)
        {
            if(slot == ability)
            {
                return slot;
            }
        }
        return null;
    }

    public AbilitySlot GetAbility(string ability_name)
    {
        foreach(var slot in abilitySlots)
        {
            if(slot.ability.name == ability_name)
            {
                return slot;
            }
        }
        return null;
    }

    public bool HasAbility(AbilitySO abilitySO, out AbilitySlot slot)
    {
        slot = GetAbility(abilitySO);

        return slot != null;
    }
    
    public bool HasAbility(AbilitySlot abilitySlot, out AbilitySlot slot)
    {
        slot = GetAbility(abilitySlot);

        return slot != null;
    }

    // Setters ============================================================================

    public void AddAbility(AbilitySO abilitySO)
    {
        if(HasAbility(abilitySO, out AbilitySlot ability))
        {
            Debug.Log($"Already have ability: {abilitySO.Name}");
            return;
        }

        AbilitySlot new_ability = new()
        {
            ability = abilitySO,
        };

        abilitySlots.Add(new_ability);
    }

    public void RemoveAbility(AbilitySlot ability)
    {
        if(!abilitySlots.Contains(ability)) return;

        abilitySlots.Remove(ability);
    }

    public void RemoveAllAbilities()
    {
        abilitySlots.Clear();
    }

    // Actions ============================================================================

    public void UpdateCooldowns()
    {
        foreach(var ability in abilitySlots)
        {
            ability.UpdateCooldown();
        }
    }

    public void ResetCooldowns()
    {
        foreach(var ability in abilitySlots)
        {
            ability.ResetCooldown();
        }
    }

    // Leftovers ============================================================================

    public void CleanUp()
    {
        abilitySlots.RemoveAll(ability => ability.IsEmpty());
    }
}

