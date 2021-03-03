using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem parts;
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        Transform playerTrans = other.GetComponent<Transform>();

        if (player) // overlapping a player object
        {
            HealthSystem playerHealth = player.GetComponent<HealthSystem>();

            if (playerHealth)
            {
                playerHealth.TakeDamage(10); //do damage
                PlayerTargeting.pHealthValue -= 10; //effect static variable to help update health bar
                Instantiate(parts, playerTrans.position, playerTrans.rotation); //spawns particles to look like blood when you get hit
            }
            Destroy(gameObject); // remove projectile
        }
    }
}
