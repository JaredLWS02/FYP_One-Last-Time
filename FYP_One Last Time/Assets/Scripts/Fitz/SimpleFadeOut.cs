using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFadeOut : MonoBehaviour
{
    CanvasGroup canvas;
    bool fadedOut = false;
    float deltaTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

        if (!fadedOut && deltaTime > 3f)
        {
            StartCoroutine(IncreaseAlphaOverTime(canvas, 1f, 0f, 2f));

            fadedOut = true;
        }
    }

    private IEnumerator IncreaseAlphaOverTime(CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        canvas.alpha = startAlpha;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            canvas.alpha = alpha;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is exactly 1 (fully opaque)
        canvas.alpha = endAlpha;
    }
}
