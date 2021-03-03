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

    HealthSystem playerHealth;

    ShootableThing isShootable;

    private float turnVar = 0;
    private float turnSpeed = 100;

    Quaternion lookingRotation;
    Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.localRotation;

        //get health vairables from this turrets script and player's script
        healthCheck = GetComponentInParent<HealthSystem>();

        playerHealth = Player.GetComponent<HealthSystem>();

        isShootable = GetComponentInParent<ShootableThing>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDeath();

        if (dyingTrigger) return; // if dead will not continue

        DistanceCheck();
        IdleAndAttackAnimation();
        
        //below keeps the Sentry from shooting at player after player is already dead
        if (playerHealth.isDying)
        {
            isClose = false;
        }

    }

    private void CheckForDeath()
    {
        if (healthCheck.isDying) // if sentry dies set trigger and keep from targeting player
        {
            dyingTrigger = true;
            isClose = false;
            //below sets it so the sentry is not a shootable target anymore for player
            isShootable.enabled = false;
        }
        //This triggers Death Animation which is the sentry spinning and shaking on the x and z rotations vigorusly
        if (dyingTrigger && dyingTimer > 0)
        {
            //Used mathf.Sin to make snetry rotate back and forth on x and z
            float betweenX = Mathf.Sin(Time.time * 40) * 25f;
            float betweenZ = Mathf.Sin(Time.time * 5) * 5f;
            turnVar += (turnSpeed + 100) * Time.deltaTime; // for y rotation
            Quaternion turnRot = Quaternion.Euler(betweenX, turnVar, betweenZ); //creates rotation
            transform.localRotation = AnimMath.Slide(transform.localRotation, turnRot, 0.001f); //smooth transition to rotation

            dyingTimer -= 1 * Time.deltaTime; //countdown
        }
        if (dyingTimer <= 0.01f)
        {
            Instantiate(parts, GunParent.position, GunParent.rotation); // this spawns death particles for sentry
        }
        if (dyingTimer <= 0) // this destroys sentry object after death animation is complete
        {
            PlayerTargeting.enemiesKilled += 1;
            Destroy(GunParent.gameObject);
        }
    }

    private void IdleAndAttackAnimation()
    {
        if (!isClose && idleTimer > 0 && !idleTrigger) // will make sentry go back to rotating idle animation if loses sight of player starting at 0,0,0
        {
            lookingRotation = Quaternion.Euler(0, 0, 0);
            transform.localRotation = AnimMath.Slide(transform.localRotation, lookingRotation, 0.09f);
            idleTimer -= 1 * Time.deltaTime;
        }
        else if (isClose) //if player is close triggers part of attack animation, aka: sentry aiming at player when close
        {
            idleTimer = 1f;
            Vector3 disToTarget = Player.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(disToTarget, Vector3.up);

            transform.localRotation = AnimMath.Slide(transform.localRotation, targetRotation, 0.001f);
        }
        else if (idleTrigger) //idle animation for sentry
        {
            turnVar += turnSpeed * Time.deltaTime;
            Quaternion dieRot = Quaternion.Euler(transform.localRotation.x, turnVar, transform.localRotation.z);
            transform.localRotation = AnimMath.Slide(transform.localRotation, dieRot, 0.01f);
        }
        if (idleTimer <= 0) //Makes it so there is a slight pause before turret goes from 0,0,0 to rotating in idle mode
        {
            idleTrigger = true;
        }
    }

    private void DistanceCheck()
    {
        if (playerHealth.isDying) return; // if player dead do not continue

        //float of distance between player and sentry
        float disCheck = Vector3.Distance(transform.position, Player.position);

        // checks value of float and determines if player is close enough for sentry to see player or not
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
