using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public enum bulletType {player, enemy};
    public bulletType type;
    public GameObject particles;
    private PlayerMovement player;

    [Header("Physics")]
    public float speed;
    public float curvature = 5;
    private Rigidbody rb;

    private void Awake()
    {
        player = GameManager.Instance.GetPlayer();
    }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!player.IsFlying)
        {
            rb.AddForce(transform.forward.normalized * speed, ForceMode.Impulse);
            rb.AddForce(transform.up * curvature, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.forward.normalized * speed, ForceMode.Impulse);
            rb.AddForce(transform.up * curvature*5, ForceMode.Impulse);
        }
        
    }
   
    // Update is called once per frame
    private void Update()
    {
        Physics.IgnoreCollision(this.GetComponentInChildren<Collider>(),FindObjectOfType<PlayerMovement>().GetComponent<Collider>());
        if (transform.forward != Vector3.zero)
        {   
            transform.forward = rb.velocity;
        }      
        Destroy(gameObject,5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (type == bulletType.enemy && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerMovement>().life--;
                Destroy(gameObject);
            }
        }
        if (type == bulletType.player && collision.gameObject.CompareTag("Enemy"))
        {
            GameObject newParticles = Instantiate(particles, transform.localPosition, Quaternion.identity);
            Destroy(newParticles, 2f);
            Destroy(gameObject);

            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (collision.gameObject.GetComponent<GroundEnemyController>())
                {
                    collision.gameObject.GetComponent<GroundEnemyController>().life--;
                    collision.gameObject.GetComponent<GroundEnemyController>().Knock();
                }
                if (collision.gameObject.GetComponent<FlyingEnemy>())
                {
                    collision.gameObject.GetComponent<FlyingEnemy>().life--;
                }
            }
        }
        else
        {
            GameObject newParticles = Instantiate(particles, transform.localPosition, Quaternion.identity);
            Destroy(newParticles, 2f);
            Destroy(gameObject);
        }

           
        
    }
}
