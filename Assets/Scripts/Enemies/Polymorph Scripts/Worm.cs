using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{
    public GameObject wormParticles;

    public void StartWormMovement()
    {
        agent.speed = 3.5f;
        GameObject dirtParticles = Instantiate(wormParticles, transform.position, Quaternion.Euler(transform.rotation.x, transform.localEulerAngles.y - 180, transform.rotation.z));
        Destroy(dirtParticles, 1.5f);
    }

    public void StopWormMovement()
    {
        agent.speed = 0;
    }
}
