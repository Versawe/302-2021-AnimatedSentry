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
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= 1 * Time.deltaTime;

        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
