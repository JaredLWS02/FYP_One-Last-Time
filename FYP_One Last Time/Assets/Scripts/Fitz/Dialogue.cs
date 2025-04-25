using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    [System.Serializable]
    public class DialogueCharacter
    {
        public string name;
        public Image charImg;
    }

    [System.Serializable]
    public class DialogueLine
    {
        public int id;

        [TextArea(3, 10)]
        public string sentence;
    }

    public List<DialogueCharacter> character = new List<DialogueCharacter>();
    public List<DialogueLine> lines = new List<DialogueLine>();
}
