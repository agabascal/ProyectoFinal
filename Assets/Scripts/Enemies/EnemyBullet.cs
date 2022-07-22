using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed;
    public float curvature = 5;
    private Rigidbody rb;
    


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward.normalized * speed, ForceMode.Impulse);
        rb.AddForce(transform.up * curvature, ForceMode.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.forward = rb.velocity;
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().life--;
            Destroy(gameObject);            
        }
    }
}
