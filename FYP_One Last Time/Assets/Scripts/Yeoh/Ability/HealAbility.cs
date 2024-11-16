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

    [Header("Temp")]
    public GameObject vfx_target;

    void TempVFX()
    {
        // flash green
        SpriteManager.Current?.FlashColor(vfx_target, -1, 1, -1);
        ModelManager.Current?.FlashColor(vfx_target, -1, 1, -1);

        Vector3 top = ColliderManager.Current.GetTop(vfx_target);
        // pop up text
        VFXManager.Current.SpawnPopUpText(top, $"+{abilitySO.magnitude}", Color.green);

        //DisableCastTrails();

        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal1, transform.position);
        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal2, transform.position);
    }
}
