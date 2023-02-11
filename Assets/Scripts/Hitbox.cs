using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public enum HitboxType { player, enemy }
    public HitboxType type;

    private void OnTriggerEnter(Collider collision)
    {
        switch (type)
        {
            case HitboxType.player:
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    AudioManager.PlayHitEnemiesAudio();
                    if (!collision.gameObject.GetComponent<BaseEnemy>().isHurt)
                    {
                        collision.gameObject.GetComponent<BaseEnemy>().isHurt = true;
                        collision.gameObject.GetComponent<BaseEnemy>().life--;
                        collision.gameObject.GetComponent<BaseEnemy>().Knock();
                    }
                }
                break;
            case HitboxType.enemy:
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (!collision.gameObject.GetComponent<PlayerMovement>().isHurt)
                    {
                        collision.gameObject.GetComponent<PlayerMovement>().isHurt = true;
                        collision.gameObject.GetComponent<PlayerMovement>().life--;
                        collision.gameObject.GetComponent<PlayerMovement>().Knock();                        
                    }
                }
                break;
        }
    }
}
