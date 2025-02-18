using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{
    public AbilitySO ability;
    public AbilityListSO abilityList;
    float cd;
    Image image;

    void Start()
    {
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // if in cooldown
        //{
        //cd = cd - Time.deltaTime;
        //image.fillAmount = abilityList.GetCooldown()/ability.cooldownTime;
        //}
        image.fillAmount = GetCurrentCd() / ability.cooldownTime;
    }

    float GetCurrentCd()
    {
        foreach (var slot in abilityList.abilitySlots)
        {
            if (slot.ability == ability)
            {
                return slot.GetCooldown();
            }
        }
        return 0;
    }
}
