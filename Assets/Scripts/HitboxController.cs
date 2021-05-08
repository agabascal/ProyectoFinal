using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{

    public enum enemyType {player, enemy}
    public enemyType type;


    private void OnTriggerEnter(Collider collision)
    {
        switch (type)
        {
            case enemyType.player:
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    if (!collision.gameObject.GetComponent<GroundEnemyController>().isHurt)
                    {
                        collision.gameObject.GetComponent<GroundEnemyController>().isHurt = true;
                        collision.gameObject.GetComponent<GroundEnemyController>().life--;
                        collision.gameObject.GetComponent<GroundEnemyController>().Knock();
                    }                    
                }
                break;
            case enemyType.enemy:
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (!collision.gameObject.GetComponent<PlayerController>().isHurt)
                    {
                        collision.gameObject.GetComponent<PlayerController>().isHurt = true;
                        collision.gameObject.GetComponent<PlayerController>().life--;
                        collision.gameObject.GetComponent<PlayerController>().Knock();
                    }                    
                }
                break;
        }
    }
}
