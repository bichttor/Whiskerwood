using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public GameObject DialogueMenu;
    public TMP_Text dialogueText, nameText;
    public Queue<string> sentences;
    public bool menuOn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menuOn)
        {
            DialogueMenu.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
        DialogueMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        nameText.text = dialogue.name;
        Debug.Log("Starting dialogue with " + dialogue.name);
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }
    public void EndDialogue()
    {
        DialogueMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      
    }

    public void FinishQuestDialogue(Dialogue dialogue)
    {
        DialogueMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        dialogueText.text = "Thank you for completing the quest!";
        nameText.text = dialogue.name;
    }
        
}
