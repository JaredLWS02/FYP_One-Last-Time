using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextSpawner : MonoBehaviour
{
    VFXManager VfxM;

    void OnEnable()
    {
        VfxM = VFXManager.Current;
    }

    // ============================================================================

    public Transform spawnpoint;
    Vector3 pos;

    public void SetPos(Vector3 to)
    {
        spawnpoint = null; // cancel spawnpoint to use custom pos
        pos = to;
    }

    // ============================================================================

    public Color color = Color.white;

    public void SetColor(Color to) => color = to;

    // ============================================================================

    public float pushForce=6;

    public void SetPushForce(float to) => pushForce = to;

    // ============================================================================

    public void Spawn(string text = "Gonk")
    {
        if(spawnpoint) pos = spawnpoint.position;

        VfxM.SpawnPopUpText(pos, text, color, pushForce);
    }
}
