using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : BaseAbility
{
    [Header("Heal Ability")]
    public HPManager hpM;
    
    // ============================================================================
    
    public override void OnAnimReleaseStart()
    {
        hpM.Add(abilitySO.magnitude);

        EventM.OnCastReleased(owner, abilitySO);

        TempVFX();
    }

    // Move to vfx manager later ============================================================================

    void TempVFX()
    {
        // flash green
        SpriteManager.Current.FlashColor(owner, -1, 1, -1);
        ModelManager.Current.FlashColor(owner, -1, 1, -1);

        Vector3 top = ColliderManager.Current.GetTop(owner);

        // pop up text
        VFXManager.Current.SpawnPopUpText(top, $"+{abilitySO.magnitude}", Color.green);

        //DisableCastTrails();

        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal1, transform.position);
        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal2, transform.position);
    }
}
