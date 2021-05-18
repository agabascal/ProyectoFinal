using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldController : MonoBehaviour
{
    public List<GameObject> enemyList;

    public bool allDefeated;

    public Animator forcefieldAnimation;
    private bool isCollapsed=false;
    public RaceGame circuitManager;

    private void Awake()
    {
        circuitManager = FindObjectOfType<RaceGame>();
    }
    // Update is called once per frame
    void Update()
    {
        
        if (enemyList.Count == 0)
        {
            allDefeated = true;
        }
        if (allDefeated && !isCollapsed)
        {
            if (forcefieldAnimation != null)
            {
                isCollapsed = true;
                forcefieldAnimation.Play("collapse");
                Destroy(gameObject, 1.5f);
                AudioManager.PlayCapsulAudio();
            }
            else
            {
                circuitManager.DisableForceField();
            }

        }     


    }
}
