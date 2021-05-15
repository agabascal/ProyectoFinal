using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCourseCheckpoint : MonoBehaviour
{

    public bool isCollected;
    public Animator anim;
    public bool isFinalCheckpoint;
    public RaceGame raceManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        raceManager = FindObjectOfType<RaceGame>();
    }    

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (isFinalCheckpoint)
            {
                raceManager.FinishRace();

            }
                isCollected = true;
                anim.SetBool("collect", true);
            raceManager.checkpointsCollected++;
            GetComponent<Collider>().enabled = false;
            
        }
    }

    public void DisableCheckpoint()
    {
        if (!isFinalCheckpoint)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            raceManager.FinishRace();
        }
          
    }
}
