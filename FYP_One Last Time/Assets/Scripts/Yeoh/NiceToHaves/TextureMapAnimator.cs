using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]

[RequireComponent(typeof(SpriteRenderer))]

public class TextureMapAnimator : MonoBehaviour
{
    public SpriteRenderer sr;

    void Reset()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
        UpdateTextureMap();
    }
    
    void Update()
    {
        if(!Application.isPlaying) return;

        UpdateInterval();
    }
    
    // ============================================================================

    [Space]
    public bool enable=true;
    public float updateInterval=.1f;
    float timer=0;

    void UpdateInterval()
    {
        if(!enable) return;

        timer += Time.unscaledDeltaTime;

        // Switch normal maps every n seconds
        if (timer >= updateInterval)
        {
            timer = 0f;

            UpdateTextureMap();
        }
    }

    // ============================================================================

    // use animator to control these
    [Header("Controlled by Animator")]
    public Sprite normalSprite;
    public Sprite emissionSprite;
    public Sprite heightSprite;

    [Header("Shader Property Names")]
    public string normalMapPropertyName = "_NormalMap";
    public string emissionMapPropertyName = "_EmissionMap";
    public string heightMapPropertyName = "_ParallaxMap";

    void UpdateTextureMap()
    {
        SetTextureMap(normalSprite, normalMapPropertyName);
        SetTextureMap(emissionSprite, emissionMapPropertyName);
        SetTextureMap(heightSprite, heightMapPropertyName);
    }

    void SetTextureMap(Sprite sprite, string property_name)
    {
        if(!sprite) return;
        if(!sr.material.HasProperty(property_name)) return;

        Texture2D tex = ConvertSpriteToTexture(sprite);

        // create material instances only in play mode
        // cant do that in edit mode because "memory leak"
        if(Application.isPlaying)
            sr.material.SetTexture(property_name, tex);
        else
            sr.sharedMaterial.SetTexture(property_name, tex);
    }

    // ============================================================================

    Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        if(!sprite) return null;

        // If the sprite uses the full texture, return it directly
        if(sprite.rect.width == sprite.texture.width && sprite.rect.height == sprite.texture.height)
        {
            return sprite.texture;
        }

        // Only create a new texture if cropping is needed
        Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        Color[] pixels = sprite.texture.GetPixels(
            Mathf.RoundToInt(sprite.textureRect.x),
            Mathf.RoundToInt(sprite.textureRect.y),
            Mathf.RoundToInt(sprite.textureRect.width),
            Mathf.RoundToInt(sprite.textureRect.height)
        );

        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }
    // https://discussions.unity.com/t/convert-sprite-image-to-texture/97618/6
}
