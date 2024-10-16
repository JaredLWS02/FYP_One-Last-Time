using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]

[RequireComponent(typeof(SpriteRenderer))]

public class SpriteNormalMapAnimator : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake()
    {
        sr=GetComponent<SpriteRenderer>();
    }

    // ============================================================================

#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    void EditorUpdate()
    {
        if(!Application.isPlaying) UpdateInterval();
    }
#endif

    // ============================================================================
    
    void Start()
    {
        UpdateNormal();
    }
    
    void Update()
    {
        if(!Application.isPlaying) return;

        UpdateInterval();
    }
    
    // ============================================================================

    [HideInInspector]
    public Sprite normalSprite; // use animator to control this

    public float updateInterval=.1f;

    float timer=0;

    public bool enable=true;

    void UpdateInterval()
    {
        if(!enable) return;

        timer += Time.unscaledDeltaTime;

        // Switch normal maps every n seconds
        if (timer >= updateInterval)
        {
            timer = 0f;

            UpdateNormal();
        }
    }

    void UpdateNormal()
    {
        if(!normalSprite) return;

        Texture2D normal_tex = ConvertSpriteToTexture2D(normalSprite);

        // Destroy the previous texture to prevent memory leaks
        TryCleanUp(currentNormalTex);

        currentNormalTex = normal_tex;

        SetNormalTex(currentNormalTex);
    }

    void SetNormalTex(Texture2D normal_tex)
    {
        // create material instances only in play mode
        // cant do that in edit mode because "memory leak"

        if(Application.isPlaying)
        {
            sr.material.SetTexture("_MainNormal", normal_tex);
        }
        else
        {
            sr.sharedMaterial.SetTexture("_MainNormal", normal_tex);
        }
    }

    // ============================================================================

    Texture2D ConvertSpriteToTexture2D(Sprite sprite)
    {
        // Create a new Texture2D with the same dimensions as the sprite's rect
        Texture2D texture = new((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);

        // Get the pixels from the sprite's texture
        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height);

        // Set the pixels to the new texture
        texture.SetPixels(pixels);
        texture.Apply(); // Apply changes to the texture

        return texture;
    }

    // ============================================================================

    // Store the current normal texture
    Texture2D currentNormalTex; 

    // Cleanup on destroy
    void OnDestroy()
    {
        TryCleanUp(currentNormalTex);
    }

    void TryCleanUp(Texture2D tex)
    {
        if(!tex) return;

        if(Application.isEditor)
        {
            DestroyImmediate(tex); // Use DestroyImmediate in edit mode
        }
        else
        {
            Destroy(tex); // Use Destroy in play mode
        }
    }
}
