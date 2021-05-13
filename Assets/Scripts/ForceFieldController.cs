using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldController : MonoBehaviour
{
    public List<GameObject> enemyList;

    public bool allDefeated;

    public Animator forcefieldAnimation;
    private bool isCollapsed=false;


    // Update is called once per frame
    void Update()
    {
        if (enemyList.Count == 0)
        {
            allDefeated = true;
        }
        if (allDefeated && !isCollapsed)
        {
            isCollapsed = true;
            forcefieldAnimation.Play("collapse");
            Destroy(gameObject,1.5f);
        }
    }
}
