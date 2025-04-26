using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesManager : MonoBehaviour
{
    public GameObject owner;
    public List<SpriteRenderer> srs = new();

    // ============================================================================

    [ContextMenu("Get Sprite Renderers")]
    public void GetSpriteRenderers()
    {
        if(!owner) return;

        srs.Clear();
        srs.AddRange(owner.GetComponents<SpriteRenderer>());
        srs.AddRange(owner.GetComponentsInChildren<SpriteRenderer>());
    }

    // ============================================================================
    
    void Awake()
    {
        if(randomStartColor)
        RandomOffsetColor();

        RecordColors();
        RecordEmissionColors();

        if(randomStartFlip)
        RandomFlip();
    }

    // ============================================================================

    Dictionary<SpriteRenderer, Color> originalColors = new();

    public void RecordColors()
    {
        foreach(var sr in srs)
        {
            // Only record if not already recorded
            if(originalColors.ContainsKey(sr)) continue;

            originalColors[sr] = sr.color;
        }
    }

    // ============================================================================

    Dictionary<Material, Color> originalEmissionColors = new();

    public void RecordEmissionColors()
    {
        foreach(var sr in srs)
        {
            Material mat = sr.material;

            if(!mat.HasProperty(emissionColorPropertyName)) continue;

            if(originalEmissionColors.ContainsKey(mat)) continue;

            Color e_color = mat.GetColor(emissionColorPropertyName);

            ColorMutator mutator = new(e_color);

            mutator.exposureValue = emissionIntensity;

            originalEmissionColors[mat] = mutator.exposureAdjustedColor;
        }
    }

    // ============================================================================

    [System.Serializable]
    public class RgbOffsetCfg
    {
        public string color_name = "Red";
        public Vector3 rgb_offset = new(.75f, -.75f, -.75f);
    }
    [Header("Offset")]
    public List<RgbOffsetCfg> rgbOffsetCfgs = new();

    RgbOffsetCfg currentRgbOffsetCfg;

    public void GetRgbOffsetCfg(string color_name)
    {
        if(string.IsNullOrEmpty(color_name))
        {
            Debug.LogWarning("RgbOffsetCfg.color_name is null or empty.");
            currentRgbOffsetCfg = null;
            return;
        }
        
        currentRgbOffsetCfg = rgbOffsetCfgs.Find(item => item.color_name == color_name);
        
        if(currentRgbOffsetCfg==null) Debug.LogWarning($"RgbOffsetCfg with name '{color_name}' not found.");
    }

    // ============================================================================

    public void OffsetColors(Vector3 rgb_offset)
    {
        RevertColors();

        Color color_offset = new(rgb_offset.x, rgb_offset.y, rgb_offset.z);

        foreach(var sr in srs)
        {
            sr.color += color_offset;
        }
    }

    public void OffsetColors() => OffsetColors(currentRgbOffsetCfg.rgb_offset);

    // ============================================================================

    public void RevertColors()
    {
        foreach(var sr in srs)
        {
            if(originalColors.ContainsKey(sr))
            {
                sr.color = originalColors[sr];
            }
        }
    }

    // ============================================================================

    [Header("Emission")]
    public bool affectEmissionColor;
    public string emissionColorPropertyName = "_EmissionColor";
    public float emissionIntensity = 3;

    // ============================================================================

    public void OffsetEmissionColors(Vector3 rgb_offset)
    {
        if(!affectEmissionColor) return;

        RevertEmissionColors();

        Color color_offset = new(rgb_offset.x, rgb_offset.y, rgb_offset.z);

        foreach(var sr in srs)
        {
            Material mat = sr.material; // sr.sharedMaterial is for global

            if(!mat.HasProperty(emissionColorPropertyName)) continue;

            Color current_e_color = mat.GetColor(emissionColorPropertyName);

            ColorMutator mutator = new(current_e_color + color_offset);

            mutator.exposureValue = emissionIntensity;

            mat.SetColor(emissionColorPropertyName, mutator.exposureAdjustedColor);
        }
    }

    public void OffsetEmissionColors() => OffsetEmissionColors(currentRgbOffsetCfg.rgb_offset);

    // ============================================================================

    public void RevertEmissionColors()
    {
        if(!affectEmissionColor) return;

        foreach(var sr in srs)
        {
            Material mat = sr.material;

            if(!mat.HasProperty(emissionColorPropertyName)) continue;

            if(!originalEmissionColors.ContainsKey(mat)) continue;

            mat.SetColor(emissionColorPropertyName, originalEmissionColors[mat]);
        }
    }

    // ============================================================================

    [Header("Flicker")]
    public float colorFlickerInterval=.05f;

    // ============================================================================

    public void FlashColors(Vector3 rgb_offset, float seconds)
    {
        if(flashing_crt!=null) StopCoroutine(flashing_crt);
        flashing_crt = StartCoroutine(FlashingColors(rgb_offset, seconds));
    }

    public void FlashColors(float seconds) => FlashColors(currentRgbOffsetCfg.rgb_offset, seconds);

    // ============================================================================

    Coroutine flashing_crt;

    IEnumerator FlashingColors(Vector3 rgb_offset, float seconds)
    {
        OffsetColors(rgb_offset);
        OffsetEmissionColors(rgb_offset);

        yield return new WaitForSeconds(seconds);

        RevertColors();
        RevertEmissionColors();
    }

    // ============================================================================

    public void ToggleColorFlicker(bool toggle, float interval)
    {
        if(colorFlickering_crt!=null) StopCoroutine(colorFlickering_crt);

        if(toggle)
        {
            colorFlickering_crt = StartCoroutine(ColorFlickering(interval));
        }
        else
        {
            RevertColors();
            RevertEmissionColors();
        }
    }

    public void ToggleColorFlicker(bool toggle) => ToggleColorFlicker(toggle, colorFlickerInterval);

    // ============================================================================

    Coroutine colorFlickering_crt;

    IEnumerator ColorFlickering(float interval)
    {
        while(true)
        {
            OffsetColors();
            OffsetEmissionColors();

            yield return new WaitForSeconds(interval);

            RevertColors();
            RevertEmissionColors();

            yield return new WaitForSeconds(interval);
        }
    }

    // ============================================================================

    [Header("Random Offset")]
    public bool randomStartColor;
    public Vector3 randomRgbOffset = new(.15f, .15f, .15f);

    public void RandomOffsetColor(Vector3 rgb_offset)
    {
        rgb_offset = new(
            Random.Range(-rgb_offset.x, rgb_offset.x),
            Random.Range(-rgb_offset.y, rgb_offset.y),
            Random.Range(-rgb_offset.z, rgb_offset.z)
        );

        OffsetColors(rgb_offset);
        OffsetEmissionColors(rgb_offset);
    }

    public void RandomOffsetColor() => RandomOffsetColor(randomRgbOffset);

    // ============================================================================

    [Header("Random Flip")]
    public bool randomStartFlip;
    public bool randomFlipY;
    public bool randomFlipX;

    public void RandomFlip(bool flipY, bool flipX)
    {
        foreach(var sr in srs)
        {
            if(flipX) sr.flipX = Random.Range(0, 2)==0;
            if(flipY) sr.flipY = Random.Range(0, 2)==0;
        }
    }
    
    public void RandomFlip() => RandomFlip(randomFlipY, randomFlipX);
}
