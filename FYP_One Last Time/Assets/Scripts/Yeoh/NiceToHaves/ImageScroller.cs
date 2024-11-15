using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    RawImage rawImg;
    public float xSpeed,ySpeed;

    void Awake()
    {
        rawImg=GetComponent<RawImage>();
    }

    void Update()
    {
        rawImg.uvRect = new Rect(rawImg.uvRect.position + new Vector2(xSpeed,ySpeed)*Time.deltaTime, rawImg.uvRect.size);
    }
}
