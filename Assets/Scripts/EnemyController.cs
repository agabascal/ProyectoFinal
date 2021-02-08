using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Navigation
    public float lookRadius = 10f;
    public Transform target;
    private NavMeshAgent agent;

    //Combat
    public int life = 3;


    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position,transform.position);

        if (distance<= lookRadius)
        {
            agent.SetDestination(target.position);
            FaceTarget();
            if (distance <= agent.stoppingDistance)
            {
                //Attack the target
                //look at the target                
            }
        }
        if (life == 0)
        {
            Destroy(gameObject);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }
}
