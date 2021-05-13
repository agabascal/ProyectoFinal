using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemyController : MonoBehaviour
{
    //Navigation
    [Header("Navigation")]
    public float lookRadius = 10f;
    public float walkpointRange;
    public Transform target;
    public NavMeshAgent agent;
    private Vector3 walkPoint;
    bool walkpointSet;
    public bool canAttack;
    public LayerMask groundLayer;

    //Combat
    [Header("Combat")]
    public int life = 3;
    public Rigidbody rb;
    public bool knockback;
    public bool isHurt;
    public bool protectsItem;
    public ForceFieldController fieldController;
    public float knockForce = 10f;
    private float distance;
    public float attackTime = 0.5f;
    protected float nextAttackTime = 0.0f;


    [Header("Animation")]
    public Animator anim;
    private bool isDead = false;



    // Start is called before the first frame update
    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
       
        target = FindObjectOfType<PlayerController>().transform;
        
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        agent.updateRotation = false;
        distance = Vector3.Distance(target.position,transform.position);
        
        if (distance <= lookRadius && canAttack)
        {
            if (agent.enabled)
            {
                agent.SetDestination(target.position - new Vector3(0,10,0));
            }
            
            FaceTarget();

            if (distance <= agent.stoppingDistance)
            {                
                //Attack the target
                anim.SetTrigger("attack");
                //look at the target    
                FaceTarget();
            }
        }
        if (distance>=10)
        {
            Patrol();
        }
        
        
        if (anim!=null)
        {
            HandleAnimation();
        }

        if (life == 0 && !isDead)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.speed = 0;
            }            
            rb.velocity = Vector3.zero;

            isDead = true;
            anim.SetTrigger("death");
        }
        
    }


    private void Patrol()
    {
        FaceTarget();
        if (!walkpointSet)
        {
            SearchWalkpoint();
        }
        else
        {
            if (agent.enabled)
            {
                agent.SetDestination(walkPoint);
            }
            
        }

        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkpoint.magnitude <1f)
        {
            walkpointSet = false;
        }
    }

    private void SearchWalkpoint()
    {
        float randomZ = Random.Range(-walkpointRange,walkpointRange);
        float randomX = Random.Range(-walkpointRange,walkpointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint,-transform.up,2f,groundLayer))
        {
            walkpointSet = true;
        }
    }
    public void EnemyDeath()
    {
        Destroy(gameObject,3f);
        if (protectsItem)
        {
            if (fieldController.enemyList.Contains(this.gameObject))
            {
                fieldController.enemyList.Remove(this.gameObject);
            }
            
        }
    }

    private void FaceTarget()
    {
         Vector3 direction = (agent.destination - transform.position).normalized;
         Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
         transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);
    }

    private void HandleAnimation()
    {
        anim.SetFloat("speed",rb.velocity.x);
        anim.SetFloat("distance",distance);
        
    }

    public void Knock()
    {
        isHurt = true;
        StartCoroutine(KnockBack());
        knockback = false;

    }
    private IEnumerator KnockBack()
    {
        
        agent.enabled = false;
        rb.isKinematic = false;                

        rb.velocity = (target.transform.forward +(Vector3.up * knockForce / 7)) * knockForce;

        yield return new WaitForSeconds(1.5f);

        agent.enabled = true;
        rb.isKinematic = true;
        isHurt = false;
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkpointRange);
    }
}
