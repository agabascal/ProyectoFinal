using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldController : MonoBehaviour
{
    public List<protectingEnemies> enemyList;

    public bool allDefeated;

    [System.Serializable]
    public class protectingEnemies
    {
        public GameObject enemy;
        public bool isDefeated;
    }
    

    // Update is called once per frame
    void Update()
    {
        foreach (protectingEnemies enemy in enemyList)
        {
            if (!enemy.isDefeated)
            {
                allDefeated = false;
            }
            else
            {

            }
        }
    }
}
