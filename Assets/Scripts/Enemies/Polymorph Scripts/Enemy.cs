using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    //Navigation
    [Header("Navigation")]
    public float lookRadius = 10f;
    public float walkpointRange;
    private float timer = 5f;
    public float navigationSpeed;
    public Transform target;
    public NavMeshAgent agent;
    private Vector3 walkPoint;
    private bool walkpointSet;
    public bool canAttack;
    public LayerMask groundLayer;
    

    //Combat
    [Header("Combat")]
    public int life = 3;
    public Rigidbody rb;
    public bool knockback;
    public bool isHurt;    
    public float knockForce = 10f;
    private float distance;
    public Hitbox hitbox;
    public GameObject deathParticles;

    [Header("Animation")]
    public Animator anim;
    public bool isDead = false;

    [Header ("Level Progression")]
    public bool protectsItem;
    public ForceFieldController fieldController;

    [Header("Sound")]
    public AudioSource enemyaudio;
    public AudioClip attackClip;

    // Start is called before the first frame update
    public void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        target = FindObjectOfType<PlayerController>().transform;

        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {
        agent.updateRotation = false;
        distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius && canAttack)
        {
            if (agent.enabled)
            {
                agent.SetDestination(target.position - new Vector3(0, 10, 0));
            }

            FaceTarget();

            if (distance <= agent.stoppingDistance)
            {
                agent.speed = 0f;
                //Attack the target
                anim.SetTrigger("attack");
                //look at the target    
                FaceTarget();
                AudioManager.PlayAttackSpiderAudio();
            }
        }
        if (distance >= 10)
        {
            Patrol();
        }


        if (anim != null)
        {
            HandleAnimation();
        }

        if (life <= 0 && !isDead)
        {

            anim.SetTrigger("death");
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.speed = 0;
            }
            rb.velocity = Vector3.zero;

            isDead = true;

        }
    }

    public virtual void Patrol()
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

        timer -= Time.deltaTime;

        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f || timer <= 0f)
        {
            walkpointSet = false;
            timer = 3f;
        }
    }

    public virtual void SearchWalkpoint()
    {
        /*if (walk != null)
        {
            walk.Play();
        }*/
        //AudioManager.PlayWalkSpiderAudio();
        float randomZ = Random.Range(-walkpointRange, walkpointRange);
        float randomX = Random.Range(-walkpointRange, walkpointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkpointSet = true;
        }
    }

    public virtual void FaceTarget()
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public virtual void HandleAnimation()
    {

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
        AudioManager.PlayHitEnemiesAudio();
        rb.velocity = (target.transform.forward + (Vector3.up * knockForce / 7)) * knockForce;

        yield return new WaitForSeconds(1.5f);

        agent.enabled = true;
        rb.isKinematic = true;
        isHurt = false;
    }
    public virtual void StartAttack()
    {
        GameObject audiosfx = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        audiosfx.AddComponent<AudioSource>().clip = attackClip;
        audiosfx.GetComponent<AudioSource>().loop = false;
        audiosfx.GetComponent<AudioSource>().Play();
        Destroy(audiosfx, 2f);
        hitbox.enabled = true;
    }

    public virtual void StopAttack()
    {
        hitbox.enabled = false;
        agent.speed = navigationSpeed;
    }

    public virtual void EnemyDeath()
    {
        Destroy(gameObject, 0.5f);
        if (protectsItem)
        {
            if (fieldController.enemyList.Contains(this.gameObject))
            {
                fieldController.enemyList.Remove(this.gameObject);
            }

        }
        if (hitbox != null)
        {
            hitbox.enabled = false;
        }

        //AudioManager.PlayDeathEnemiesAudio();
        GetComponent<Collider>().enabled = false;
        AudioManager.PlayDeathEnemiesAudio();
        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particles, 5f);
    }
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkpointRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(KnockBack());
            life--;
        }

    }
}
