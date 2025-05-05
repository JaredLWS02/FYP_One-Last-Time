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
    int counter = 1;
    float transitionDuration = 1f;
    bool textEnd = false, textAppear = true;

    private void Update()
    {
        if (!textEnd && (textAppear && InputManager.Current.jumpKeyDown))
        {
            if (counter < textSet.Length)
            {
                int imageIndex = GetImageIndexForText(counter - 1);
                int nextImageIndex = GetImageIndexForText(counter);
                // Fade in the text prompt
                StartCoroutine(DecreaseAlphaOverTime(textSet[counter - 1], transitionDuration, imageIndex, nextImageIndex));
                textAppear = false;
            }
        }

        if ((textEnd && Input.anyKeyDown) || InputManager.Current.dashKeyDown)
        {
            ScenesManager.Current.LoadNextScene();
        }
    }

    // Function to get the corresponding image index based on the text prompt index
    private int GetImageIndexForText(int index)
    {
        switch (index)
        {
            case 0: return -1;
            case 1: return 0;
            case 2: return 0;
            case 3: return 0;
            case 4: return -1;
            case 5: return 1;
            case 6: return 1;
            case 7: return 1;
            case 8: return 2;
            case 9: return 2;
            case 10: return 2;
            case 11: return -1;
            case 12: return 3;
            case 13: return 3;
            case 14: return 3;
            case 15: return 3;
            case 16: return 3;
            case 17: return -1;
            default: return -1;
        }
    }

    // Coroutine to smoothly transition the alpha value from startAlpha to endAlpha over duration for text
    private IEnumerator IncreaseAlphaOverTime(TextMeshProUGUI text, float duration, int imageIndex, int nextImageIndex)
    {
        float elapsedTime = 0f;

        Color startTextColor = text.color;
        startTextColor.a = 0.0f;
        text.color = startTextColor;
        if (nextImageIndex != -1 && nextImageIndex != imageIndex)
        {
            Color startImageColor = imageSet[nextImageIndex].color;
            startImageColor.a = 0.0f;
            imageSet[nextImageIndex].color = startImageColor;
        }

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / duration);
            Color newTextColor = text.color;
            newTextColor.a = alpha;
            text.color = newTextColor;

            if (nextImageIndex != -1 && nextImageIndex != imageIndex)
            {
                Color newImageColor = imageSet[nextImageIndex].color;
                newImageColor.a = alpha;
                imageSet[nextImageIndex].color = newImageColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is exactly 1 (fully opaque)
        Color finalTextColor = text.color;
        finalTextColor.a = 1.0f;
        text.color = finalTextColor;
        if (nextImageIndex != -1 && nextImageIndex != imageIndex)
        {
            Color finalImageColor = imageSet[nextImageIndex].color;
            finalImageColor.a = 1.0f;
            imageSet[nextImageIndex].color = finalImageColor;
        }

        textAppear = true;
    }

    // Does the same thing as IncreaseAlphaOverTime, just some added stuff at the end
    private IEnumerator DecreaseAlphaOverTime(TextMeshProUGUI text, float duration, int imageIndex, int nextImageIndex)
    {
        float elapsedTime = 0f;

        Color startTextColor = text.color;
        startTextColor.a = 1.0f;
        text.color = startTextColor;
        if (imageIndex != -1 && nextImageIndex != imageIndex)
        {
            Color startImageColor = imageSet[imageIndex].color;
            startImageColor.a = 1.0f;
            imageSet[imageIndex].color = startImageColor;
        }

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / duration);
            Color newTextColor = text.color;
            newTextColor.a = alpha;
            text.color = newTextColor;

            if (imageIndex != -1 && nextImageIndex != imageIndex)
            {
                Color newImageColor = imageSet[imageIndex].color;
                newImageColor.a = alpha;
                imageSet[imageIndex].color = newImageColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color finalTextColor = text.color;
        finalTextColor.a = 0.0f;
        text.color = finalTextColor;
        if (imageIndex != -1 && nextImageIndex != imageIndex)
        {
            Color finaImageColor = imageSet[imageIndex].color;
            finaImageColor.a = 0.0f;
            imageSet[imageIndex].color = finaImageColor;
        }

        StopAllCoroutines();
        StartCoroutine(IncreaseAlphaOverTime(textSet[counter], transitionDuration, imageIndex, nextImageIndex));
        if (counter == textSet.Length - 1)
            textEnd = true;
        else
            counter++;
    }
}

