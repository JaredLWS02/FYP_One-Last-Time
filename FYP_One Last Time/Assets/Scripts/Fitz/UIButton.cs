using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    Button buttonComp;

    private void Start()
    {
        buttonComp = GetComponent<Button>();
    }

    public void OnClick()
    {
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxUISelectButton, transform.position, false);
    }

    public void OnHover()
    {
        if (buttonComp.interactable)
            AudioManager.Current.PlaySFX(SFXManager.Current.sfxUIHoverButton, transform.position, false);
    }

    public void OnQuit()
    {
        ScenesManager.Current.Quit();
    }

    public void PlayGame()
    {
        ScenesManager.Current.LoadNextScene();
    }
}
