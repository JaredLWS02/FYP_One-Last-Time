using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilitySlot
{
    public AbilitySO ability;

    public bool IsEmpty() => ability==null;

    // ============================================================================

    float cooldownLeft;
    
    public void DoCooldown() => cooldownLeft = ability.cooldownTime;

    public bool IsCooling() => cooldownLeft>0;

    public void UpdateCooldown()
    {
        if(IsCooling())
        cooldownLeft -= Time.deltaTime;

        if(cooldownLeft<0) cooldownLeft=0;
    }

    public void CancelCooldown() => cooldownLeft=0;
};

// ============================================================================

[CreateAssetMenu(fileName="New Ability List", menuName="SO/Ability/AbilityListSO")]

public class AbilityListSO : ScriptableObject
{
    public List<AbilitySlot> abilitySlots = new();

    // Getters ============================================================================

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

    public bool HasAbility(string ability_name, out AbilitySlot slot)
    {
        slot = GetAbility(ability_name);
        return slot != null;
    }

    public bool HasAbility(string ability_name)
    {
        return HasAbility(ability_name, out var slot);
    }
    
    public bool HasAbility(AbilitySO abilitySO, out AbilitySlot slot)
    {
        slot = GetAbility(abilitySO);
        return slot != null;
    }

    public bool HasAbility(AbilitySO abilitySO)
    {
        return HasAbility(abilitySO, out var slot);
    }
    
    // Setters ============================================================================

    public void AddAbility(AbilitySO abilitySO)
    {
        if(HasAbility(abilitySO))
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

    public void CancelCooldowns()
    {
        foreach(var ability in abilitySlots)
        {
            ability.CancelCooldown();
        }
    }

    // Leftovers ============================================================================

    public void CleanUp()
    {
        abilitySlots.RemoveAll(ability => ability.IsEmpty());
    }
}

