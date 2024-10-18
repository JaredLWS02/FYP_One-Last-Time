using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]

[RequireComponent(typeof(SpriteRenderer))]

public class SpriteNormalMapAnimator : MonoBehaviour
{
    SpriteRenderer sr;

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

    // ============================================================================

    void UpdateNormal()
    {
        if(!normalSprite) return;

        Texture2D normal_tex = ConvertSpriteToTexture(normalSprite);

        SetNormalTex(normal_tex);
    }


    void SetNormalTex(Texture2D normal_tex)
    {
        // create material instances only in play mode
        // cant do that in edit mode because "memory leak"
        if(Application.isPlaying)
            sr.material.SetTexture("_MainNormal", normal_tex);
        else
            sr.sharedMaterial.SetTexture("_MainNormal", normal_tex);
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
                Debug.Log(colors.Length+"_"+ newColors.Length);
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
