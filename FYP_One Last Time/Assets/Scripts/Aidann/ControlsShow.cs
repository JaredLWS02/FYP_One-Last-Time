using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsShow : MonoBehaviour
{
    public TextMeshProUGUI controlsText;
    public TextMeshProUGUI hideShowText;

    private Vector3 originalPosition;
    private bool controlsVisible = true;

    void Start()
    {
        originalPosition = hideShowText.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            controlsVisible = !controlsVisible;
            controlsText.enabled = controlsVisible;

            if (controlsVisible)
            {
                hideShowText.transform.position = originalPosition;
                hideShowText.text = "| P | <b> Hide <b>";
            }
            else
            {
                hideShowText.transform.position = controlsText.transform.position;
                hideShowText.text = "| P | <b> Show <b>";
            }
        }
    }
}
