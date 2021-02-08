using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    public float curvature=5;
    public GameObject particles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.up * curvature, ForceMode.Impulse);
        rb.AddForce(transform.forward.normalized * speed, ForceMode.Impulse);
        rb.AddForce(transform.up * curvature, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.forward = rb.velocity;
        Destroy(gameObject,5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            GameObject newParticles = Instantiate(particles, transform.localPosition, Quaternion.identity);
            Destroy(newParticles, 2f);
            Destroy(gameObject);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (collision.gameObject.GetComponent<EnemyController>())
                {
                    collision.gameObject.GetComponent<EnemyController>().life--;
                }
                if (collision.gameObject.GetComponent<FlyingEnemy>())
                {
                    collision.gameObject.GetComponent<FlyingEnemy>().life--;
                }
            }
        }
    }
}
