using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBoost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position += Vector3.up * 500 * Time.deltaTime;
            Debug.Log("boost");
        }
    }
}
