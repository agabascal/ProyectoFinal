﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBoost : MonoBehaviour
{
    public enum boostType {Vertical, Horizontal }
    public boostType type;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == boostType.Vertical)
            {
                other.GetComponent<CharacterController>().Move(Vector3.Lerp(other.transform.position, Vector3.up, 50));

            }
            else
            {
                other.GetComponent<PlayerController>().isBoosted = true;
            }
          
        }
    }
}