using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using Player;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;
    public Text dialogueText;
    private bool canContinue;
    public GameObject continueMarker;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canContinue)
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
        if (GameManager.Instance.treeDialogue != null)
        {
            if (GameManager.Instance.treeDialogue.index == 0)
            {
                GameManager.Instance.treeDialogue.index++;
                GameManager.Instance.dialoguePanel.SetActive(false);
                FindObjectOfType<PlayerMovement>().canMove = true;
                Camera.main.transform.GetComponent<CinemachineBrain>().enabled = true;
            }
            else if (GameManager.Instance.partsCollected == 4)
            {
                StartCoroutine(GameManager.Instance.FadeOut());
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindObjectOfType<RaceGame>().dialoguePanel.SetActive(false);
            GameManager.Instance.fadeImage = GameManager.Instance.blackFadeImage;
            StartCoroutine(GameManager.Instance.FadeOut());
        }
        
    }

    private IEnumerator WriteText(string sentence)
    {
        continueMarker.SetActive(false);
        canContinue = false;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        canContinue = true;
        continueMarker.SetActive(true);
    }
}
