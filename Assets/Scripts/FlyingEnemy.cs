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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(target.position,transform.position)<detectRadius)
        {
            transform.LookAt(target.position);
            agent.SetDestination(target.position);
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
