using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaunch : MonoBehaviour
{
    Rigidbody rb;

    private float thrust = 25;
    private float lifeTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //rigidbody of bullet
        rb = GetComponent<Rigidbody>();

        //once bullet spawns apply the force once on start
        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        //this will despawn the bullet after 5 seconds for clean-up
        lifeTimer -= 1 * Time.deltaTime;

        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
