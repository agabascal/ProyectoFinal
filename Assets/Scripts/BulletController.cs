using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public enum bulletType {player, enemy};
    public bulletType type;
    public GameObject particles;

    [Header("Physics")]
    public float speed;
    public float curvature = 5;
    private Rigidbody rb;        

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!FindObjectOfType<PlayerController>().isFlying)
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
                collision.gameObject.GetComponent<PlayerController>().life--;
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

        if (collision.gameObject.layer == 8)
        {
            GameObject newParticles = Instantiate(particles, transform.localPosition, Quaternion.identity);
            Destroy(newParticles, 2f);
            Destroy(gameObject);
        }
    }
}
