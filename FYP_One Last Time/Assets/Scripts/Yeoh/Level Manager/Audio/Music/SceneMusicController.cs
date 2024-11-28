using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    SceneMusic sceneMusic;

    void OnEnable()
    {
        sceneMusic = SceneMusic.Current;
    }

    // ============================================================================

    public void SetDefaultLayer(int to) => sceneMusic.SetDefaultLayer(to);

    public void SetDefaultLayer(string layer_name) => sceneMusic.SetDefaultLayer(layer_name);

    public void ChangeToDefaultLayer() => sceneMusic.ChangeToDefaultLayer();

    public void ChangeToLayerName(string layer_name) => sceneMusic.ChangeToLayerName(layer_name);
}
