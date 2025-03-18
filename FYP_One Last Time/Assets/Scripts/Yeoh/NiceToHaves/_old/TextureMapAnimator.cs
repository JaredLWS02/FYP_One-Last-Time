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
    public Sprite occlusionSprite;
    public Sprite heightSprite;
    public Sprite specularSprite;
    public Sprite emissionSprite;

    [Header("Shader Property Names")]
    public string normalMapPropertyName = "_NormalMap";
    public string occlusionMapPropertyName = "_OcclusionMap";
    public string heightMapPropertyName = "_ParallaxMap";
    public string specularMapPropertyName = "_SpecGlossMap";
    public string emissionMapPropertyName = "_EmissionMap";

    void UpdateTextureMap()
    {
        SetTextureMap(normalSprite, normalMapPropertyName);
        SetTextureMap(occlusionSprite, occlusionMapPropertyName);
        SetTextureMap(heightSprite, heightMapPropertyName);
        SetTextureMap(specularSprite, specularMapPropertyName);
        SetTextureMap(emissionSprite, emissionMapPropertyName);
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
        try
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] colors = newText.GetPixels();
                Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                                                                (int)System.Math.Ceiling(sprite.textureRect.y),
                                                                (int)System.Math.Ceiling(sprite.textureRect.width),
                                                                (int)System.Math.Ceiling(sprite.textureRect.height));
                //Debug.Log(colors.Length+"_"+ newColors.Length);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }catch
        {
            return sprite.texture;
        }
    }

    // https://discussions.unity.com/t/convert-sprite-image-to-texture/97618/6
}
