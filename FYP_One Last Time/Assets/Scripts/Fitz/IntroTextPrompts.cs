using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroTextPrompts : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] textSet;
    float deltaTime;
    int counter = 0;
    float transitionDuration = 3f;
    bool textEnd = false;

    private void Update()
    {
        deltaTime += Time.deltaTime;

        if (!textEnd && (deltaTime > 7 || Input.anyKeyDown))
        {
            if (counter < textSet.Length)
            {
                // Increase alpha of vertex color of textSet[counter] from 0 to 255 within 3 seconds
                StartCoroutine(IncreaseAlphaOverTime(textSet[counter], 0f, 1f, transitionDuration));

                if (counter == textSet.Length - 1)
                    textEnd = true;
                else
                    counter++;
            }

            deltaTime = 0;
        }

        if (textEnd && Input.anyKeyDown)
        {
            ScenesManager.Current.LoadNextScene();
        }
    }

    // Coroutine to smoothly transition the alpha value from startAlpha to endAlpha over duration
    private IEnumerator IncreaseAlphaOverTime(TextMeshProUGUI text, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        Color startColor = text.color;
        startColor.a = startAlpha;
        text.color = startColor;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            Color newColor = text.color;
            newColor.a = alpha;
            text.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is exactly 1 (fully opaque)
        Color finalColor = text.color;
        finalColor.a = endAlpha;
        text.color = finalColor;
    }
}
