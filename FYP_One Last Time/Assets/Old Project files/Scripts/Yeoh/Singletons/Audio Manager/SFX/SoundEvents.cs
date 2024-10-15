using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundEvents : MonoBehaviour
{
    public UnityEvent OnEnabled;
    public UnityEvent OnDisabled;

    void OnEnable()
    {
        OnEnabled.Invoke();
    }   
    void OnDisable()
    {
        OnDisabled.Invoke();
    }   
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    // public void PlaySfxFireIgnite()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxFireIgnite, transform.position);
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxFireIgnite2, transform.position);
    // }
    // public void LoopSfxFireLoop()
    // {
    //     looping = AudioManager.Current.LoopSFX(gameObject, SFXManager.Current.sfxFireLoop);
    // }

    AudioSource looping;

    public void StopLoop()
    {
        if(looping) AudioManager.Current.StopLoop(looping);
    }

    // public void PlaySfxMaceCharge()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxMaceCharge, transform.position);
    // }

    // public void PlaySfxUIBook()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIBook, transform.position, false);
    // }

    // public void PlaySfxBowDraw()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxBowDraw, transform.position);
    // }
    // public void PlaySfxBowShoot()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxBowShoot, transform.position);
    // }

    // public void PlaySfxFstNpc()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxFstNpc, transform.position);
    // }
    // public void PlaySfxFstSkeleton()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxFstSkeleton, transform.position);
    // }
    // public void PlaySfxFstSpider()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxFstSpider, transform.position);
    // }
    // public void PlaySfxFstZombie()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxFstZombie, transform.position);
    // }

    // public void PlaySfxHitArrow()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxHitArrow, transform.position);
    // }
    // public void PlaySfxHitFire()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxHitFire, transform.position);
    // }
    // public void PlaySfxHitFist()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxHitFist, transform.position);
    // }
    // public void PlaySfxHitSword()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxHitSword, transform.position);
    // }
    // public void PlaySfxHitTool()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxHitTool, transform.position);
    // }
    
    // public void PlaySfxSwingFist()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxSwingFist, transform.position);
    // }
    // public void PlaySfxSwingTool()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxSwingTool, transform.position);
    // }
    
    // public void PlaySfxThrow()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxThrow, transform.position);
    // }
    
    // public void PlaySfxUIInvClose()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIInvClose, transform.position, false);
    // }
    // public void PlaySfxUIInvOpen()
    // {
    //     AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIInvOpen, transform.position, false);
    // }
    
    
}
