﻿using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class XrayCamera : MonoBehaviour
{

    private Transform player;
    private Transform[] obstructions;
    private int oldHitsNumber;
    public LayerMask objectMask;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        ObjectTransparency();
    }

    private void ObjectTransparency()
    {
        float distanceToPlayer = Vector3.Distance(transform.position,player.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position,player.transform.position - transform.position,distanceToPlayer,objectMask);

        if (hits.Length > 0)
        {
            int newHits = hits.Length - oldHitsNumber;

            if (obstructions != null && obstructions.Length > 0 && newHits < 0)
            {
                // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
                for (int i = 0; i < obstructions.Length; i++)
                {                    
                    //obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    //obstructions[i].GetComponent<MeshRenderer>().material.color = new Color(obstructions[i].GetComponent<MeshRenderer>().material.color.r,
                      //                                                                      obstructions[i].GetComponent<MeshRenderer>().material.color.g,
                        //                                                                    obstructions[i].GetComponent<MeshRenderer>().material.color.b,
                          //                                                                  1f); 
                }
            }
            obstructions = new Transform[hits.Length];
            // Hide the current obstructions 
            for (int i = 0; i < hits.Length; i++)
            {
                Transform obstruction = hits[i].transform;
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                //obstruction.GetComponent<MeshRenderer>().material.color = new Color(obstruction.GetComponent<MeshRenderer>().material.color.r,
                   //                                                                         obstruction.GetComponent<MeshRenderer>().material.color.g,
                     //                                                                       obstruction.GetComponent<MeshRenderer>().material.color.b,
                                                                                        //    0.25f);
                obstructions[i] = obstruction;
            }
            oldHitsNumber = hits.Length;
        }
        else
        {   // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
            if (obstructions != null && obstructions.Length > 0)
            {
                for (int i = 0; i < obstructions.Length; i++)
                {
                    obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    //obstructions[i].GetComponent<MeshRenderer>().material.color = new Color(obstructions[i].GetComponent<MeshRenderer>().material.color.r,
                    //                                                                        obstructions[i].GetComponent<MeshRenderer>().material.color.g,
                      //                                                                      obstructions[i].GetComponent<MeshRenderer>().material.color.b,
                                                                                    //        1f);
                }
                oldHitsNumber = 0;
                obstructions = null;
            }
        }
    }
}
