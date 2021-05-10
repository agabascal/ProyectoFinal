using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceGame : MonoBehaviour
{
    [Header("Variables")]

    public PlayerController player;
    public float timer;
    public GameObject[] checkpoints;
    public Transform startPosition;
    public bool raceWon;
    public FlyingCourseCheckpoint[] checkpointObjects;
    public int checkpointsCollected=0;

    [Header("UI Elements")]
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        timer = 10;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = "Time: "+ ((int)timer);

        if (timer<=0 && !raceWon)
        {
            player.GetComponent<CharacterController>().enabled = false;
            Vector3 startpos = startPosition.position;
            player.transform.position = startpos;
            player.GetComponent<CharacterController>().enabled = true;

        }
        if (raceWon)
        {
            player.shootingUnlocked = true;
        }
        else
        {
            player.transform.position = startPosition.position;
        }
    }

    public void FinishRace()
    {
        if (checkpointsCollected >= (int)(checkpointObjects.Length * 0.75f))
        {
            Debug.Log((int)(checkpointObjects.Length * 0.75f));
            raceWon = true;
        }
    }    
}
