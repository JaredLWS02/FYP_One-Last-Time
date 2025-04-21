//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class IntroTextPrompts : MonoBehaviour
//{
//    [SerializeField] TextMeshProUGUI[] textSet;
//    float deltaTime;
//    int counter = 0;
//    float transitionDuration = 3f;
//    bool textEnd = false;

//    private void Update()
//    {
//        deltaTime += Time.deltaTime;

//        if (!textEnd && (deltaTime > 7 || Input.anyKeyDown))
//        {
//            if (counter < textSet.Length)
//            {
//                // Increase alpha of vertex color of textSet[counter] from 0 to 255 within 3 seconds
//                StartCoroutine(IncreaseAlphaOverTime(textSet[counter], 0f, 1f, transitionDuration));

//                if (counter == textSet.Length - 1)
//                    textEnd = true;
//                else
//                    counter++;
//            }

//            deltaTime = 0;
//        }

//        if (textEnd && Input.anyKeyDown)
//        {
//            ScenesManager.Current.LoadNextScene();
//        }
//    }

//    // Coroutine to smoothly transition the alpha value from startAlpha to endAlpha over duration
//    private IEnumerator IncreaseAlphaOverTime(TextMeshProUGUI text, float startAlpha, float endAlpha, float duration)
//    {
//        float elapsedTime = 0f;

//        Color startColor = text.color;
//        startColor.a = startAlpha;
//        text.color = startColor;

//        while (elapsedTime < duration)
//        {
//            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
//            Color newColor = text.color;
//            newColor.a = alpha;
//            text.color = newColor;

//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        // Ensure the final alpha is exactly 1 (fully opaque)
//        Color finalColor = text.color;
//        finalColor.a = endAlpha;
//        text.color = finalColor;
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroTextPrompts : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] textSet; // Array for text prompts
    [SerializeField] Image[] imageSet; // Array for images corresponding to the text prompts
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
                // Determine the corresponding image for the current text
                int imageIndex = GetImageIndexForText(counter);

                // If the image index is valid, fade in the image
                if (imageIndex != -1)
                {
                    StartCoroutine(FadeImageIn(imageSet[imageIndex], transitionDuration));
                }

                // Fade in the text prompt
                StartCoroutine(IncreaseAlphaOverTime(textSet[counter], 0f, 1f, transitionDuration));

                // Wait before starting to fade out (optional: you can modify this delay as needed)
                StartCoroutine(FadeOutAfterDelay(imageIndex, counter, 5f)); // 5 seconds is just an example delay

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

    // Function to get the corresponding image index based on the text prompt index
    private int GetImageIndexForText(int index)
    {
        switch (index)
        {
            case 0: return 0; // Text 1 -> Image 1
            case 1: return 1; // Text 2 -> Image 2
            case 2: return 2; // Text 3 -> Image 3
            case 3: return -1; // Text 4 -> No image
            case 4: return 3; // Text 5 -> Image 4
            case 5: return 3; // Text 6 -> Image 4
            case 6: return 4; // Text 7 -> Image 5
            case 7: return 5; // Text 8 -> Image 6
            case 8: return 6; // Text 9 -> Image 7
            case 9: return 6; // Text 10 -> Image 7
            case 10: return -1; // Text 11 -> No image
            case 11: return 7; // Text 12 -> Image 8
            case 12: return 8; // Text 13 -> Image 9
            case 13: return 9; // Text 14 -> Image 10
            case 14: return 9; // Text 15 -> Image 10
            case 15: return 9; // Text 16 -> Image 10
            case 16: return -1; // Text 17 -> No image
            default: return -1;
        }
    }

    // Coroutine to smoothly transition the alpha value from startAlpha to endAlpha over duration for text
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

    // Coroutine to fade in an image
    private IEnumerator FadeImageIn(Image image, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = image.color;
        startColor.a = 0f; // Start with the image invisible
        image.color = startColor;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is 1 (fully opaque)
        Color finalColor = image.color;
        finalColor.a = 1f;
        image.color = finalColor;
    }

    // Coroutine to fade out an image
    private IEnumerator FadeImageOut(Image image, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = image.color;
        startColor.a = 1f; // Start with the image visible
        image.color = startColor;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is 0 (fully transparent)
        Color finalColor = image.color;
        finalColor.a = 0f;
        image.color = finalColor;
    }

    // Coroutine to fade out the image and text after a delay
    private IEnumerator FadeOutAfterDelay(int imageIndex, int textIndex, float delay)
    {
        // Wait for the specified delay before starting fade-out
        yield return new WaitForSeconds(delay);

        // Fade out the text
        StartCoroutine(IncreaseAlphaOverTime(textSet[textIndex], 1f, 0f, transitionDuration));

        // If the image is still visible, fade it out
        if (imageIndex != -1 && GetImageIndexForText(counter + 1) != imageIndex)
        {
            StartCoroutine(FadeImageOut(imageSet[imageIndex], transitionDuration));
        }
    }
}

