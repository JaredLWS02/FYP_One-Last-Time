using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelectButton : MonoBehaviour
{
    public void SelectButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
}
