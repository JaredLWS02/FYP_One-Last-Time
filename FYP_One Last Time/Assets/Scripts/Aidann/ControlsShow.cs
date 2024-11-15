using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsShow : MonoBehaviour
{
    public TextMeshProUGUI controlsText;

    private bool controlsVisible = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            controlsVisible = !controlsVisible;

            if (controlsVisible)
            {
                controlsText.text = "- WASD/Arrow Keys to Move\r\n- Space/A to Jump\r\n- C/B to Dash\r\n- Z/X to Light Attack\r\n- X/Y to Heavy Attack\r\n- Q/LB to Parry\r\n- E+Z/RB+Y to Heal\r\n|P| <b>Hide<b>";
            }
            else
            {
                controlsText.text = "| P | <b> Show<b>";
            }
        }
    }
}
