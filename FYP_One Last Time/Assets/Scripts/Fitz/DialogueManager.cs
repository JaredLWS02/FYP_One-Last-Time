using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image characterImage;

    public Animator animator;

    public Pilot playerPilot, ratPilot;
    public GameObject background;

    //private Queue<string> sentences;
    private Queue<Dialogue.DialogueLine> sentences;
    private List<Dialogue.DialogueCharacter> characters;

    bool isFirstSentence = false, inDialogue = false;

    // Use this for initialization
    void Start()
    {
        //sentences = new Queue<string>();
        sentences = new Queue<Dialogue.DialogueLine>();
    }

    void Update()
    {
        if (InputManager.Current.dashKeyDown)
        {
            StopAllCoroutines();
            EndDialogue();
        }
        if (InputManager.Current.jumpKeyDown && inDialogue)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        inDialogue = true;
        animator.SetBool("IsOpen", true);
        playerPilot.currentPilot = PilotType.None;
        ratPilot.currentPilot = PilotType.None;
        background.SetActive(true);

        //nameText.text = dialogue.name;

        sentences.Clear();
        characters = dialogue.character;

        //foreach (string sentence in dialogue.sentences)
        foreach (Dialogue.DialogueLine line in dialogue.lines)
        {
            sentences.Enqueue(line);
        }

        isFirstSentence = true;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        dialogueText.text = "";

        if (characterImage != null)
        {
            SpriteAlphaZero(characterImage);
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        //string sentence = sentences.Dequeue();
        Dialogue.DialogueLine line = sentences.Dequeue();

        Dialogue.DialogueCharacter speaker = characters.Find(c => characters.IndexOf(c) == line.id);

        if (speaker != null)
        {
            nameText.text = speaker.name;
            characterImage = speaker.charImg;
            //characterImage.color = SpriteAlphaOne(characterImage);
            SpriteAlphaOne(characterImage);
        }

        else
        {
            Debug.LogWarning("Character ID not found: " + line.id);
            nameText.text = "Unknown";
        }

        //nameText.text = line.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        if (isFirstSentence)
        {
            yield return new WaitForSeconds(0.5f);
            isFirstSentence = false;
        }

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        //yield return new WaitForSeconds(3.0f);
        //DisplayNextSentence();
    }

    void EndDialogue()
    {
        inDialogue = false;
        animator.SetBool("IsOpen", false);
        SpriteAlphaZero(characterImage);
        playerPilot.currentPilot = PilotType.Player;
        ratPilot.currentPilot = PilotType.AI;
        background.SetActive(false);
    }

    void SpriteAlphaOne(Image image)
    {
        Color temp = image.color;

        if (temp.a < 1f)
        {
            temp.a = 1f;
            image.color = temp;
        }
    }

    void SpriteAlphaZero(Image image)
    {
        Color temp = image.color;

        if (temp.a > 0f)
        {
            temp.a = 0f;
            image.color = temp;
        }
    }

}
