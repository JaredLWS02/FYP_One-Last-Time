using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGReceiver : MonoBehaviour
{
    public float fadeSpeed;
    public float riseSpeed;
    public int ID;
    bool isFading = false;
    ARGScript signal;
    private void Start()
    {
        signal = gameObject.transform.root.GetComponent<ARGScript>(); // Get ARGScript component
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.transform.root.CompareTag("Player"))
        {
            StartCoroutine(FadeOut(this.gameObject)); // Object fades out and rises upwards
            signal.ID = ID; // Initialize ID
            signal.CopyPngFile(); // Run copy function
        }
    }

    IEnumerator FadeOut(GameObject obj)
    {
        if (!isFading)
            isFading = true;
        while (this.GetComponent<Renderer>().material.color.a > 0)
        {
            // Fade out stuff
            Color objectColor = this.GetComponent<Renderer>().material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            this.GetComponent<Renderer>().material.color = objectColor;

            // Object rise stuff
            Vector3 pos = obj.transform.position;
            pos.y += (riseSpeed * Time.deltaTime);
            obj.transform.position = pos;

            yield return null;
        }
    }
}
