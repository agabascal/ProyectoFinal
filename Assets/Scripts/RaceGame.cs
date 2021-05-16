using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceGame : MonoBehaviour
{
    [Header("Variables")]

    public PlayerController player;
    public GameObject[] enemyIsland;
    public GameObject[] islandFields;
    public int islandIndex=0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        islandIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {        
        if (Vector3.Distance(player.transform.position,enemyIsland[islandIndex].transform.position) <= 20f && islandIndex < enemyIsland.Length-1)
        {
            if (player.state == PlayerController.playerState.flight)
            {
                player.anim.Play("Land");
                player.state = PlayerController.playerState.ground;
            }
            
            islandFields[islandIndex].SetActive(true);
            Invoke("ActivateCollider",0.5f);
        }

        if (islandIndex == enemyIsland.Length-1)
        {
            islandFields[islandIndex].SetActive(false);
        }
    }

    public void DisableForceField()
    {
        islandFields[islandIndex].SetActive(false);
        islandIndex++;
    }

    public void ActivateCollider()
    {
        islandFields[islandIndex].GetComponent<Collider>().isTrigger = false;
    }
}
