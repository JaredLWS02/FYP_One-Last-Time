using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{
    public AbilitySO ability;
    public AbilityListSO abilityList;
    AbilitySlot slot;
    Image image;

    void Start()
    {
        image = this.GetComponent<Image>();
        slot = GetAbilitySlot();
    }

    // Update is called once per frame
    void Update()
    {
        if (slot == null || ability.cooldownTime == 0)
            image.fillAmount = 0;
        else
            image.fillAmount = slot.GetCooldown() / ability.cooldownTime;
    }

    AbilitySlot GetAbilitySlot()
    {
        foreach (var slot in abilityList.abilitySlots)
        {
            if (slot.ability == ability)
            {
                return slot;
            }
        }
        return null;
    }
}
