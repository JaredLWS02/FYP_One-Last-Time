using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinRandomizer : MonoBehaviour
{
    public List<GameObject> skinList = new List<GameObject>();
    [HideInInspector] public GameObject chosenSkin;
    public int skin;

    void Awake()
    {
        skin = Random.Range(0,skinList.Count);
        chosenSkin = skinList[skin];

        for(int i=skinList.Count-1; i>=0; i--)
        {
            if(skinList[i]!=chosenSkin)
            {
                Destroy(skinList[i]);
                skinList.Remove(skinList[i]);
            }
            else skinList[i].SetActive(true);
        }
    }
}
