using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioLayerController : MonoBehaviour
{
    protected AudioLayerManager layerM;

    void OnEnable()
    {
        OnBaseEnable();

        events.OnEnable?.Invoke();
    }

    protected virtual void OnBaseEnable(){}

    void OnDisable() => events.OnDisable?.Invoke();

    // ============================================================================

    AudioLayer selectedLayer;

    public void ChooseLayerName(string layer_name)
        => selectedLayer = layerM.GetLayerByName(layer_name);

    // ============================================================================

    AudioSO selectedSO;

    public void ChooseAudioSO(AudioSO so) => selectedSO = so;

    // ============================================================================

    public void ChangeSO(float fadeOutTime)
        => layerM.ChangeSO(selectedLayer, selectedSO, fadeOutTime);

    // ============================================================================

    public float crossfadeOutTime=3;
    public float crossfadeWaitTime=1;
    public float crossfadeInTime=3;

    public void SetCrossfadeOutTime(float t) => crossfadeOutTime = t;
    public void SetCrossfadeWaitTime(float t) => crossfadeWaitTime = t;
    public void SetCrossfadeInTime(float t) => crossfadeInTime = t;

    public void CrossfadeToLayer(string layer_name)
        => layerM.CrossfadeToLayer(layer_name, crossfadeOutTime, crossfadeWaitTime, crossfadeInTime);

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
    }
    [Space]
    public Events events;
}
