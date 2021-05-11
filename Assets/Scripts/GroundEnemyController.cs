using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemyController : MonoBehaviour
{
    //Navigation
    [Header("Navigation")]
    public float lookRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;

    //Combat
    [Header("Combat")]
    public int life = 3;
    public Rigidbody rb;
    public bool knockback;
    public float knockForce = 10f;
    public bool isHurt;
    private float distance;

    [Header("Animation")]
    public Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        distance = Vector3.Distance(target.position,transform.position);
        
        if (distance <= lookRadius)
        {
            if (agent.enabled)
            {
                agent.SetDestination(target.position);
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
        
        if (anim!=null)
        {
            HandleAnimation();
        }
        
    }

    public void Death()
    {
        Invoke("Destroy(gameObject)",3f);        
    }
    private void FaceTarget()
    {
        /* Vector3 direction = (target.position - transform.position).normalized;
         Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
         transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);*/
        transform.LookAt(target);
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

        rb.velocity = (-transform.forward +(Vector3.up * knockForce / 7)) * knockForce;

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
    }
}
