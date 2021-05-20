using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Aranya : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        target = FindObjectOfType<PlayerController>().transform;

        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        agent.updateRotation = false;
        float distance = Vector3.Distance(target.position, transform.position);

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

}
