using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{

    //Navigation
    public Transform target;

    public Transform shootPoint;
    public GameObject bullet;

    public NavMeshAgent agent;

    public float detectRadius, attackRadius;

    //Combat
    public int life = 3;
    private float nextAttackTime;
    public float attackRate;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position,target.position)<detectRadius)
        {
            transform.LookAt(target.position);
            agent.SetDestination(target.position);

            if (Vector3.Distance(transform.position, target.position) < attackRadius)
            {
                if (Time.time>= nextAttackTime)
                {
                    Instantiate(bullet, shootPoint.position, transform.rotation);
                    nextAttackTime = Time.time + attackRate;
                }                
            }
            
        }
        if (life==0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,detectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRadius);
    }
}
