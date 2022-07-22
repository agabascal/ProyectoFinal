using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class FlightCameraControl : MonoBehaviour
{
    public Transform target;

    private Vector3 pos;
    public float offset = 10f;
    public float distance = 5f;
    public float smooth;
    private float bias = 0.96f;
    

    // Start is called before the first frame update
    private void Start()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    private void Update()
    {
        pos = (target.position - target.forward * offset) + (Vector3.up * distance);

        Vector3 nextPos = transform.position * bias + (pos * (1.0f - bias));

        transform.position = Vector3.Slerp(transform.position,nextPos,smooth);

        transform.LookAt(target.position + target.forward * 30f);
    }
}
