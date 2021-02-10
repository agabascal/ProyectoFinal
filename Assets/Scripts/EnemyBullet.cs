using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed;
    private Rigidbody rb;
    public float curvature = 5;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward.normalized * speed, ForceMode.Impulse);
        rb.AddForce(transform.up * curvature, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = rb.velocity;
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().life--;
            Destroy(gameObject);            
        }
    }
}
