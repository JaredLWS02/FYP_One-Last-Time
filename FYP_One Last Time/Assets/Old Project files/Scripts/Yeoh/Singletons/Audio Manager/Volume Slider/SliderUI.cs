using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    Slider slider;
    public TextMeshProUGUI numberTMP;

    void Awake()
    {
        slider=GetComponent<Slider>();
    }

    void Start()
    {
        ChangeNumberText(slider.value);
    }

    public void ChangeNumberText(float num)
    {
        if(round) numberTMP.text = $"{Round(num, decimalPlaces)}";
        else numberTMP.text = $"{num}";
    }
    
    public bool round=true;
    public int decimalPlaces=1;

    float Round(float num, int decimalPlaces)
    {
        int factor=1;

        for(int i=0; i<decimalPlaces; i++)
        {
            factor *= 10;
        }

        return Mathf.Round(num * factor) / (float)factor;
    }

    public void PlaySfxTest()
    {
        //AudioManager.Current.PlaySFX(SFXManager.Current.Enemy1HurtClips, transform.position, false);
    }
}
