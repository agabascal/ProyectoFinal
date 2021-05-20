using System.Collections;
using System.Collections.Generic;
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
                    if (!collision.gameObject.GetComponent<Enemy>().isHurt)
                    {
                        collision.gameObject.GetComponent<Enemy>().isHurt = true;
                        collision.gameObject.GetComponent<Enemy>().life--;
                        collision.gameObject.GetComponent<Enemy>().Knock();
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
                    }
                }
                break;
        }
    }
}
