using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontColorChanger : MonoBehaviour
{
    public Color brightColor;
    public Color darkColor;
    public TMP_Text text;

    public void ColorOnHover()
    {
        text.color = brightColor;
    }

    public void ColorOffHover()
    {
        text.color = darkColor;
    }
}
