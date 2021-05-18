using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{

    public enum HitboxType {player, enemy}
    public HitboxType type;


    private void OnTriggerEnter(Collider collision)
    {
        switch (type)
        {
            case HitboxType.player:
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    AudioManager.PlayHitEnemiesAudio();
                    if (!collision.gameObject.GetComponent<GroundEnemyController>().isHurt)
                    {
                        collision.gameObject.GetComponent<GroundEnemyController>().isHurt = true;
                        collision.gameObject.GetComponent<GroundEnemyController>().life--;
                        collision.gameObject.GetComponent<GroundEnemyController>().Knock();
                    }                    
                }
                break;
            case HitboxType.enemy:
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (!collision.gameObject.GetComponent<PlayerController>().isHurt)
                    {
                        collision.gameObject.GetComponent<PlayerController>().isHurt = true;
                        collision.gameObject.GetComponent<PlayerController>().life--;
                        collision.gameObject.GetComponent<PlayerController>().Knock();                       
                        //collision.GetComponent<Rigidbody>().velocity = -collision.transform.forward*5;*/
                    }                    
                }
                break;
        }
    }
}
