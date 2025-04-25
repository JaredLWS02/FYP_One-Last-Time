using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour
{
    public CanvasGroup[] groups;
    int counter = 1;
    float transitionDuration = 1.5f;
    bool end = false, appear = true;

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Current.jumpKeyDown && !end && appear)
        {
            appear = false;
            StartCoroutine(DecreaseAlphaOverTime(groups[counter - 1], transitionDuration));
        }

        if ((InputManager.Current.jumpKeyDown && end) || InputManager.Current.dashKeyDown)
        {
            ScenesManager.Current.LoadMainMenu();
        }
    }

    private IEnumerator DecreaseAlphaOverTime(CanvasGroup group, float duration)
    {
        float elapsedTime = 0f;

        group.alpha = 1.0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / duration);
            group.alpha = alpha;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        group.alpha = 0.0f;

        StopAllCoroutines();
        StartCoroutine(IncreaseAlphaOverTime(groups[counter], transitionDuration));
        if (counter == groups.Length - 1)
            end = true;
        else
            counter++;
    }

    private IEnumerator IncreaseAlphaOverTime(CanvasGroup group, float duration)
    {
        float elapsedTime = 0f;

        group.alpha = 0.0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / duration);
            group.alpha = alpha;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        group.alpha = 1.0f;

        appear = true;
    }
}
