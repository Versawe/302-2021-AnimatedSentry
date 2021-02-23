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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
            shootDelay += 1* Time.deltaTime;

            if (shootDelay >= shootDelayMax)
            {
                Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                shootDelay = 0;
            }
        }
        else
        {
            shootDelay = 0;
        }
    }
}
