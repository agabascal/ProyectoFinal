using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightCameraControl : MonoBehaviour
{
    public Transform target;

    private Vector3 pos;
    public float offset = 10f;
    public float distance = 5f;
    private float bias = 0.96f;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {


        pos = (target.position - target.forward * offset) + (Vector3.up * distance);

        transform.position = transform.position*bias + (pos * (1.0f - bias));



        transform.LookAt(target.position + target.forward * 30f);
    }
}
