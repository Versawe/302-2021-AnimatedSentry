using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTarget : MonoBehaviour
{

    public Transform Player;
    public Transform GunParent;
    public ParticleSystem parts;

    public bool isClose = false;
    private bool idleTrigger = false;
    private bool dyingTrigger = false;
    private float dyingTimer = 3.5f;
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
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDeath();

        if (dyingTrigger) return;

        DistanceCheck();
        IdleAndAttackAnimation();

    }

    private void CheckForDeath()
    {
        if (healthCheck.isDying)
        {
            dyingTrigger = true;
            isClose = false;
        }
        if (dyingTrigger && dyingTimer > 0)
        {
            float betweenX = Mathf.Sin(Time.time * 40) * 25f;
            float betweenZ = Mathf.Sin(Time.time * 5) * 5f;
            turnVar += (turnSpeed + 100) * Time.deltaTime;
            Quaternion turnRot = Quaternion.Euler(betweenX, turnVar, betweenZ);
            transform.localRotation = AnimMath.Slide(transform.localRotation, turnRot, 0.001f);

            dyingTimer -= 1 * Time.deltaTime;
        }
        if (dyingTimer <= 0.01f)
        {
            Instantiate(parts, GunParent.position, GunParent.rotation);
        }
        if (dyingTimer <= 0)
        {
            Destroy(GunParent.gameObject);
        }
    }

    private void IdleAndAttackAnimation()
    {
        if (!isClose && idleTimer > 0 && !idleTrigger)
        {
            lookingRotation = Quaternion.Euler(0, 0, 0);
            transform.localRotation = AnimMath.Slide(transform.localRotation, lookingRotation, 0.09f);
            idleTimer -= 1 * Time.deltaTime;
        }
        else if (isClose)
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
            transform.localRotation = AnimMath.Slide(transform.localRotation, dieRot, 0.01f);
        }
        if (idleTimer <= 0)
        {
            idleTrigger = true;
        }
    }

    private void DistanceCheck()
    {
        float disCheck = Vector3.Distance(transform.position, Player.position);

        if (disCheck <= 11)
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
