using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using Player;


public class RaceGame : MonoBehaviour
{
    [Header("Variables")]

    public PlayerMovement player;
    public GameObject[] enemyIsland;
    public GameObject[] islandFields;
    public GameObject[] targets;
    public GameObject[] npcs;
    public GameObject guideArrow;
    public GameObject dialoguePanel;
    public int islandIndex=0;
    public int targetIndex=0;
    public int distanceIndex=0;
    public Transform target;
    public float distanceToTarget;
    public float[] distanceToIsland;
    public bool playerArrived;
    public DialogueTrigger finalIslandDialogue;

    // Start is called before the first frame update
    void Start()
    {        
        player = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
        islandIndex = 0;
        targetIndex = 0;
        target = targets[targetIndex].transform;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(player.transform.position, target.position);
        if (Vector3.Distance(player.transform.position, enemyIsland[islandIndex].transform.position) <= distanceToIsland[distanceIndex] && islandIndex < enemyIsland.Length - 1)
        {
            if (player.state == PlayerMovement.PlayerState.Flight)
            {
                distanceIndex++;
                player.anim.Play("Land");
                player.state = PlayerMovement.PlayerState.Ground;
                guideArrow.SetActive(false);
            }

            islandFields[islandIndex].SetActive(true);
            Invoke("ActivateCollider", 0.5f);
        }

        if (Vector3.Distance(player.transform.position, targets[targetIndex].transform.position) <= 35f && targetIndex < targets.Length)
        {
            targetIndex++;
            target = targets[targetIndex].transform;
        }


        guideArrow.transform.LookAt(target);

        if (islandIndex==3)
        {
            islandFields[3].SetActive(false) ;
        }
        if (islandIndex == 3 && targetIndex == 16 && Vector3.Distance(player.transform.position, enemyIsland[3].transform.position) <= 50)
        {
            if (!playerArrived)
            {
                player.anim.Play("Land");
                player.state = PlayerMovement.PlayerState.Ground;
                guideArrow.SetActive(false);
                player.canMove = false;
                for (int i = 0; i < npcs.Length; i++)
                {
                    npcs[i].GetComponent<NpcController>().walkPointSet = true;
                    npcs[i].GetComponent<NpcController>().walkPoint = player.transform.position;                    
                    npcs[i].GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                    npcs[i].GetComponent<NavMeshAgent>().stoppingDistance = 8f;
                    npcs[i].GetComponent<NavMeshAgent>().isStopped = false;
                    npcs[i].GetComponent<NavMeshAgent>().speed = 2f;
                }

                dialoguePanel.SetActive(true);
                finalIslandDialogue.TriggerDialogue(finalIslandDialogue.dialogue[0]);
                playerArrived = true;
            }
        }

        if ((player.transform.position.x < -8000f || player.transform.position.x > -300) || (player.transform.position.y < 4800f|| player.transform.position.y > 7000) || (player.transform.position.z > 3800f || player.transform.position.z <-1200))
        {
            GameManager.Instance.fadeImage = GameManager.Instance.whiteFadeImage;
            StartCoroutine(GameManager.Instance.FadeOut());
            StartCoroutine(playerDeath());
        }
    }

    public void DisableForceField()
    {
        islandFields[islandIndex].SetActive(false);
        islandIndex++;
        targetIndex++;
        guideArrow.SetActive(true);
        player.life = 3;
        target = targets[targetIndex].transform;
    }

    public void ActivateCollider()
    {
        if (islandFields[islandIndex].GetComponent<Collider>().isTrigger)
        {
            islandFields[islandIndex].GetComponent<Collider>().isTrigger = false;
        }        
    }

    public IEnumerator playerDeath()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
