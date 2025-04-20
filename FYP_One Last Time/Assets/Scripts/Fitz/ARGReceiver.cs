using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGReceiver : MonoBehaviour
{
    public float fadeSpeed;
    public float riseSpeed;
    public int ID;
    ARGScript signal;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.transform.root.CompareTag("Player"))
        {
            FadeOut(col.gameObject); // Object fades out
            // Object rises upwards
            if (col.gameObject.transform.root.GetComponent<ARGScript>() != null)
            {
                signal = col.gameObject.transform.root.GetComponent<ARGScript>(); // Get ARGScript component
                signal.ID = ID; // Initialize ID
                signal.CopyPngFile(); // Run copy function
            }
        }
    }

    IEnumerator FadeOut(GameObject obj)
    {
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
