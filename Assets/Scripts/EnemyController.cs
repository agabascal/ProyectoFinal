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
    public Rigidbody rb;
    private bool knockback;
    public float knockForce = 10f;


    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position,transform.position);

        if (distance<= lookRadius)
        {
            if (agent.enabled)
            {
                agent.SetDestination(target.position);
            }
            
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

        if (knockback)
        {
            StartCoroutine(KnockBack());
            knockback = false;
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);
    }


    private IEnumerator KnockBack()
    {
        agent.enabled = false;
        rb.isKinematic = false;

        

        rb.velocity = (-transform.forward +(Vector3.up * knockForce / 7)) * knockForce;

        yield return new WaitForSeconds(1.5f);

        agent.enabled = true;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(KnockBack());
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }
}
