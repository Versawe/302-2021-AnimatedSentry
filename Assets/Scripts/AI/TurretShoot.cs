using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletSpawn;

    private float shootDelay = 0;
    private float shootDelayMax = 1f;
    private bool isShooting = false;

    // Update is called once per frame
    void Update()
    {
        // grabs variable from another script to see if player is close to turret
        //changes is shooting to trigger something
        if (gameObject.GetComponent<TurretTarget>().isClose)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }

        SpawnBullet();
    }

    private void SpawnBullet()
    {
        if (isShooting)
        {
            shootDelay += 1* Time.deltaTime; // need to increase shoot delay var

            if (shootDelay >= shootDelayMax)
            {
                //spawns bullet and resets shooting delay
                Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                shootDelay = 0;
                // This is for the shooting Animation, to Jolt the turret back on each gun shot
                transform.localEulerAngles += new Vector3(-15, 0, 0);
            }
        }
        else
        {
            //keeps delay at 0 if not shooting, so first shot is delayed like the rest
            shootDelay = 0;
        }
    }
}
