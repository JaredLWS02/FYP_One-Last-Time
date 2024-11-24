using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class SpriteFlipNormalFix : MonoBehaviour
{
    private SpriteRenderer sr;
    private MaterialPropertyBlock propertyBlock;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        
        propertyBlock = new MaterialPropertyBlock();
    }

    void LateUpdate()
    {
        sr.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat("_FlipX", sr.flipX ? -1f : 1f);
        propertyBlock.SetFloat("_FlipY", sr.flipY ? -1f : 1f);

        sr.SetPropertyBlock(propertyBlock);
    }
}
