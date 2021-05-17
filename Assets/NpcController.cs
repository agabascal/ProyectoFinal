using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{

    private NavMeshAgent agent;
    public float walkPointRange = 8f;
    private bool walkPointSet;
    private Vector3 walkPoint;
    public LayerMask groundLayer;
    private float timer = 5f;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Roam();
        
    }

    private void Roam()
    {
        if (!walkPointSet)
        {
            anim.SetBool("isStopped", agent.isStopped);
            Invoke("SearchWalkPoint",3f);
        }
        else
        {
            agent.SetDestination(walkPoint);
        }

        timer -= Time.deltaTime;

        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f || timer <= 0f)
        {
            agent.isStopped = true;
            anim.SetBool("isStopped", agent.isStopped);
            walkPointSet = false;
            timer = 5f;
        }
    }
    private void SearchWalkPoint()
    {
        agent.isStopped = false;
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 4f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,walkPointRange);
    }
}
