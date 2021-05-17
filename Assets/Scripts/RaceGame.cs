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
    public GameObject[] targets;
    public GameObject guideArrow;
    public int islandIndex=0;
    public int targetIndex=0;
    public Transform target;
    public float distanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        islandIndex = 0;
        targetIndex = 0;
        target = targets[targetIndex].transform;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(player.transform.position, target.position);
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

        if (Vector3.Distance(player.transform.position, targets[targetIndex].transform.position) <= 20f && targetIndex < targets.Length)
        {
            targetIndex++;
            target = targets[targetIndex].transform;
        }

        if (targetIndex == 2)
        {
            target = enemyIsland[islandIndex].transform;
        }

        if (islandIndex == enemyIsland.Length-1)
        {
            islandFields[islandIndex].SetActive(false);
        }

        guideArrow.transform.LookAt(target);
    }

    public void DisableForceField()
    {
        islandFields[islandIndex].SetActive(false);
        islandIndex++;
        target = targets[targetIndex].transform;
    }

    public void ActivateCollider()
    {
        islandFields[islandIndex].GetComponent<Collider>().isTrigger = false;
    }
}
