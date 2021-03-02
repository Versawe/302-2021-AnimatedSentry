using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTarget : MonoBehaviour
{

    public Transform Player;
    public Transform GunParent;

    public bool isClose = false;
    private bool idleTrigger = false;
    private bool dyingTrigger = false;
    private float dyingTimer = 2f;
    private float idleTimer = 1f;

    HealthSystem healthCheck;

    private float turnVar = 0;
    private float turnSpeed = 100;

    Quaternion lookingRotation;
    Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.localRotation;
        healthCheck = GetComponentInParent<HealthSystem>();
        dyingTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceCheck();


        if (!isClose && idleTimer > 0 && !idleTrigger && !dyingTrigger)
        {
            lookingRotation = Quaternion.Euler(0, 0, 0);
            transform.localRotation = AnimMath.Slide(transform.localRotation, lookingRotation, 0.01f);
            idleTimer -= 1 * Time.deltaTime;
        }
        else if(isClose && !dyingTrigger)
        {
            idleTimer = 1f;
            Vector3 disToTarget = Player.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(disToTarget, Vector3.up);

            transform.localRotation = AnimMath.Slide(transform.localRotation, targetRotation, 0.001f);
        }
        else if (idleTrigger)
        {
            turnVar += turnSpeed * Time.deltaTime;
            Quaternion dieRot = Quaternion.Euler(transform.localRotation.x, turnVar, transform.localRotation.z);
            transform.localRotation = AnimMath.Slide(transform.localRotation, dieRot, 0.001f);
        }
        if (idleTimer <= 0)
        {
            idleTrigger = true;
        }

        if (healthCheck.isDying)
        {
            dyingTrigger = true;
        }
        if (dyingTrigger && dyingTimer > 0)
        {
            float betweenX = AnimMath.Lerp(-90, 90, 0.001f);
            float betweenZ = AnimMath.Lerp(-15, 15, 0.001f);
            turnVar += (turnSpeed * turnSpeed) * Time.deltaTime;
            Quaternion turnRot = Quaternion.Euler(betweenX, turnVar, betweenZ);
        }

    }

    private void DistanceCheck()
    {
        float disCheck = Vector3.Distance(transform.position, Player.position);

        if (disCheck <= 10)
        {
            isClose = true;
            idleTrigger = false;
            idleTimer = 1f;
            turnVar = 0;
        }
        else
        {
            isClose = false;
        }
    }
}
