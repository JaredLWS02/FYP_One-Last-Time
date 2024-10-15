using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Vector4 offsetColor = new Vector4(.15f, .15f, .15f, 0);
    public Vector2 randomFlip = new Vector2(1, 0);

    void Start()
    {
        if(offsetColor!=Vector4.zero)
        SpriteManager.Current.RandomOffsetColor(gameObject, offsetColor.x, offsetColor.y, offsetColor.z, offsetColor.w);

        if(randomFlip!=Vector2.zero)
        SpriteManager.Current.RandomFlip(gameObject, randomFlip.y!=0, randomFlip.x!=0);
    }
}
