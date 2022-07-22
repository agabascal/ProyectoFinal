using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player;

public class FlyingEnemy : MonoBehaviour
{

    //Navigation
    [Header("Navigation")]
    public Transform target;    
    public GameObject bullet;
    public NavMeshAgent agent;
    public float detectRadius, attackRadius;

    //Combat
    [Header("Combat")]
    public int life = 3;
    private float nextAttackTime;
    public float attackRate;
    public Transform shootPoint;


    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    private void Update()
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
