using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;
    public Text dialogueText;
    private bool canContinue;
    

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& canContinue)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        StopAllCoroutines();
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StartCoroutine(WriteText(sentence));
    }

    public void EndDialogue()
    {
        GameManager.Instance.dialoguePanel.SetActive(false);
        FindObjectOfType<PlayerController>().canMove = true;
        Camera.main.transform.GetComponent<CinemachineBrain>().enabled = true;
    }

    private IEnumerator WriteText(string sentence)
    {
        canContinue = false;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        canContinue = true;
    }
}
