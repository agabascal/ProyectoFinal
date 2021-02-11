using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartController : MonoBehaviour
{

    public int id;
    private bool iscollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!iscollected)
            {
                GameManager.Instance.UpdateCollected(id);
                iscollected = true;
                Destroy(gameObject);
            }
            
        }
    }
}
