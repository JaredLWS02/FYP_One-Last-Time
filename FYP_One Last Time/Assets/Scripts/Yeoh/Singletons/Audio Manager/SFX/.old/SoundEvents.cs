// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;

// public class SoundEvents : MonoBehaviour
// {
//     AudioManager AudioM;
//     SFXManager SfxM;

//     public UnityEvent OnEnableEvent;
//     public UnityEvent OnDisableEvent;

//     void OnEnable()
//     {
//         AudioM = AudioManager.Current;
//         SfxM = SFXManager.Current;

//         OnEnableEvent?.Invoke();
//     }   
//     void OnDisable()
//     {
//         OnDisableEvent?.Invoke();
//     }   
    
//     // ============================================================================

//     AudioSource loopSource;

//     public void StopLoop()
//     {
//         AudioM.StopLoop(loopSource);
//     }

//     // ============================================================================
    
//     // public void PlaySfxFireIgnite()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxFireIgnite, transform.position);
//     //     AudioM.PlaySFX(SfxM.sfxFireIgnite2, transform.position);
//     // }
//     // public void LoopSfxFireLoop()
//     // {
//     //     looping = AudioM.LoopSFX(gameObject, SfxM.sfxFireLoop);
//     // }

//     // ============================================================================

//     // public void PlaySfxMaceCharge()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxMaceCharge, transform.position);
//     // }

//     // ============================================================================

//     // public void PlaySfxUIBook()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxUIBook, transform.position, false);
//     // }

//     // ============================================================================

//     // public void PlaySfxBowDraw()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxBowDraw, transform.position);
//     // }
//     // public void PlaySfxBowShoot()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxBowShoot, transform.position);
//     // }

//     // ============================================================================

//     // public void PlaySfxFstNpc()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxFstNpc, transform.position);
//     // }
//     // public void PlaySfxFstSkeleton()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxFstSkeleton, transform.position);
//     // }
//     // public void PlaySfxFstSpider()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxFstSpider, transform.position);
//     // }
//     // public void PlaySfxFstZombie()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxFstZombie, transform.position);
//     // }

//     // ============================================================================

//     // public void PlaySfxHitArrow()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxHitArrow, transform.position);
//     // }
//     // public void PlaySfxHitFire()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxHitFire, transform.position);
//     // }
//     // public void PlaySfxHitFist()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxHitFist, transform.position);
//     // }
//     // public void PlaySfxHitSword()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxHitSword, transform.position);
//     // }
//     // public void PlaySfxHitTool()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxHitTool, transform.position);
//     // }
    
//     // ============================================================================

//     // public void PlaySfxSwingFist()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxSwingFist, transform.position);
//     // }
//     // public void PlaySfxSwingTool()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxSwingTool, transform.position);
//     // }
    
//     // ============================================================================

//     // public void PlaySfxThrow()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxThrow, transform.position);
//     // }
    
//     // ============================================================================

//     // public void PlaySfxUIInvClose()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxUIInvClose, transform.position, false);
//     // }
//     // public void PlaySfxUIInvOpen()
//     // {
//     //     AudioM.PlaySFX(SfxM.sfxUIInvOpen, transform.position, false);
//     // }
    
    
// }
