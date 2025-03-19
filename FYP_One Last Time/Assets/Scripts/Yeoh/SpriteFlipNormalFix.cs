using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class SpriteFlipNormalFix : MonoBehaviour
{
    SpriteRenderer sr;
    MaterialPropertyBlock propertyBlock;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        propertyBlock = new MaterialPropertyBlock();
    }

    // ============================================================================

    public string flipXPropertyName = "_FlipX";
    public string flipYPropertyName = "_FlipY";

    void LateUpdate()
    {
        sr.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat(flipXPropertyName, sr.flipX ? -1f : 1f);
        propertyBlock.SetFloat(flipYPropertyName, sr.flipY ? -1f : 1f);

        sr.SetPropertyBlock(propertyBlock);
    }
}
