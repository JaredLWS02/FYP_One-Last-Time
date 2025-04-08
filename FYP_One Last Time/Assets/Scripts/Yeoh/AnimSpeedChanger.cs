using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpeedChanger : MonoBehaviour
{
    public Animator anim;

    public string speedPropertyName = "Attack Speed";

    public void SetAnimSpeed(float to) => anim.SetFloat(speedPropertyName, to);
}
