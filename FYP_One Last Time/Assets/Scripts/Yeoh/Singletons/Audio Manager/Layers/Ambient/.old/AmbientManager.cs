using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientManager : RandomUpdate
{
    AudioSource ambSource;
    float defaultVolume;

    void Awake()
    {
        ambSource = GetComponent<AudioSource>();
        defaultVolume = ambSource.volume;
        ambSource.loop=false; // no loop because need to detect if done to change clip
    }

    // ============================================================================

    [Header("Ambient Manager")]
    public bool ambLoopEnabled=true;
    public AudioClip[] defaultLoopClips;

    void Start()
    {
        if(HasClips(defaultLoopClips))
            SwapAmbLoop(defaultLoopClips);
    }

    // ============================================================================

    List<AudioClip> currentLoopClips = new();

    public void SwapAmbLoop(AudioClip[] clips)
    {
        if(currentLoopClips.Count>0)
            currentLoopClips.Clear();
        
        if(!HasClips(clips)) return;

        currentLoopClips.AddRange(clips);
        RestartAmbLoop();
    }

    void RestartAmbLoop()
    {
        if(currentLoopClips.Count<=0)
        {
            ambSource.Stop();
            return;
        }

        ambSource.volume = defaultVolume;
        ambSource.clip = currentLoopClips[Random.Range(0, currentLoopClips.Count)];
        ambSource.Play();
    }

    // ============================================================================

    protected override void Update()
    {
        base.Update();
        
        if(!ambLoopEnabled) return;
        if(ambSource.isPlaying) return;

        RestartAmbLoop();
    }

    // ============================================================================

    public void ChangeAmbLoop(AudioClip[] clips, float fadeOutTime=3)
    {
        if(changingAmb_crt!=null) StopCoroutine(changingAmb_crt);
        changingAmb_crt = StartCoroutine(ChangingAmbLoop(clips, fadeOutTime));
    }
    
    Coroutine changingAmb_crt;

    IEnumerator ChangingAmbLoop(AudioClip[] clips, float fadeOutTime)
    {
        AudioManager.Current.TweenVolume(ambSource, 0, fadeOutTime);
        yield return new WaitForSecondsRealtime(fadeOutTime);
        if(HasClips(clips)) SwapAmbLoop(clips);
    }

    // ============================================================================

    public void ToggleAmbLoop(bool toggle, float fadeTime=3)
    {
        ambLoopEnabled = toggle;

        AudioClip[] clips = toggle ? defaultLoopClips : null;

        ChangeAmbLoop(clips, fadeTime);
    }

    public void ToggleAmbShort(bool toggle) => enableSlowUpdate=toggle;

    // ============================================================================

    public bool HasClips(AudioClip[] clips) => clips!=null && clips.Length>0;
}
