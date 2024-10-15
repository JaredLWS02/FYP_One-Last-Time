using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientManager : MonoBehaviour
{
    AudioSource ambSource;
    float defVolume;

    void Awake()
    {
        ambSource = GetComponent<AudioSource>();
        defVolume = ambSource.volume;
        ambSource.loop=false;
    }

    [Header("Ambient")]
    public bool ambEnabled=true;
    public AudioClip[] ambLong;

    List<AudioClip> currentClips = new List<AudioClip>();

    void Start()
    {
        if(HasClips(ambLong)) SwapAmb(ambLong);
    }

    void RestartAmb()
    {
        if(currentClips.Count>0)
        {
            ambSource.volume = defVolume;
            ambSource.clip = currentClips[Random.Range(0, currentClips.Count)];
            ambSource.Play();
        }
    }

    public void SwapAmb(AudioClip[] clips)
    {
        if(currentClips.Count>0)
        {
            currentClips.Clear();
        }
        
        if(HasClips(clips))
        {
            currentClips.AddRange(ambLong);
            RestartAmb();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        if(ambEnabled) UpdateShuffleMusic();
    }

    void UpdateShuffleMusic()
    {
        if(!ambSource.isPlaying) RestartAmb();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    public void ChangeAmb(AudioClip[] clips, float fadeOutTime=3)
    {
        AudioManager.Current.TweenVolume(ambSource, 0, fadeOutTime);

        if(changingAmbRt!=null) StopCoroutine(changingAmbRt);
        changingAmbRt = StartCoroutine(ChangingAmb(clips, fadeOutTime));
    }
    
    Coroutine changingAmbRt;
    IEnumerator ChangingAmb(AudioClip[] clips, float fadeOutTime)
    {
        if(fadeOutTime>0) yield return new WaitForSecondsRealtime(fadeOutTime);
        if(HasClips(clips)) SwapAmb(clips);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    public void PlayAmb(AudioClip[] clips)
    {
        ChangeAmb(clips, 0);
    }

    public void StopAmb(float fadeOutTime=3)
    {
        ChangeAmb(null, fadeOutTime);

        if(currentClips.Count>0)
        {
            currentClips.Clear();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Short Ambient")]
    public bool ambShortEnabled=true;
    public AudioClip[] ambShort;
    public Vector2 ambShortInterval = new Vector2(2, 10);

    void OnEnable()
    {
        StartShortAmb();
    }

    public void StartShortAmb()
    {
        randAmbRt = StartCoroutine(RandAmb());
    }

    Coroutine randAmbRt;
    IEnumerator RandAmb()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(ambShortInterval.x, ambShortInterval.y));

            if(ambShortEnabled)
            {
                for(int i=0; i<Random.Range(1, 3); i++)
                {   
                    PlayShortAmb(ambShort);
                }
            }
        }
    }

    public void PlayShortAmb(AudioClip[] clips)
    {
        if(clips.Length==0) return;
        
        AudioManager.Current.PlaySFX(clips, transform.position, false, true, Random.Range(-1f, 1f), Random.Range(.1f, 1));
    }

    public void StopShortAmb()
    {
        if(randAmbRt!=null) StopCoroutine(randAmbRt);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool HasClips(AudioClip[] clips)
    {
        return clips!=null && clips.Length>0;
    }
}
