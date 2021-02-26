using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerTargeting : MonoBehaviour
{
    public List<ShootableThing> potientalTargets = new List<ShootableThing>();

    public Transform target;
    public bool wantsToTarget = false;
    public bool wantsToAttack = false;

    public float visionDis = 10;

    float cooldownScan = 0;

    float cooldownPick = 0;
    public float visionAngle = 80;

    float cooldownShoot = 0;
    private float roundsPerSecond = 10;

    //player's bones
    public Transform armL;
    public Transform armR;

    private Vector3 startPosArmL;
    private Vector3 startPosArmR;

    public Transform handL;
    public Transform handR;
    public ParticleSystem flash;

    public CameraOrbit camOrbit;

    void Start()
    {
        //set this at end of project it's annoying while developing
        Cursor.lockState = CursorLockMode.Locked;

        startPosArmL = armL.localPosition;
        startPosArmR = armR.localPosition;

        camOrbit = Camera.main.GetComponentInParent<CameraOrbit>();
    }

    void Update()
    {
        if (!wantsToTarget) target = null;

        wantsToTarget = Input.GetButton("Fire2");
        wantsToAttack = Input.GetButton("Fire1");

        cooldownScan -= Time.deltaTime; // counting down

        if (cooldownScan <= 0 || target == null && wantsToTarget) ScanForTargets(); // do this when count down finished

        cooldownPick -= Time.deltaTime;

        if (cooldownPick <= 0) PickATarget();


        if (target && !CanSeeThing(target)) target = null;

        if(cooldownShoot > 0) cooldownShoot -= Time.deltaTime;

        SlideArmsHome();

        DoAttack();
    }

    private void SlideArmsHome()
    {
        armL.localPosition = AnimMath.Slide(armL.localPosition, startPosArmL, 0.01f);
        armR.localPosition = AnimMath.Slide(armR.localPosition, startPosArmR, 0.01f);
    }

    private void DoAttack()
    {
        if (cooldownShoot > 0) return; // too soon!
        if (!wantsToTarget) return; //player not targeting
        if (!wantsToAttack) return; // p not shooting
        if (!target) return; // no target

        HealthSystem health = target.GetComponent<HealthSystem>();

        if (health)
        {
            health.TakeDamage(20);
        }
        if (!CanSeeThing(target)) return;

        camOrbit.Shake(1);

        cooldownShoot = 1 / roundsPerSecond;
        // attack!
        Instantiate(flash, handL.position, handL.rotation);
        Instantiate(flash, handR.position, handR.rotation);
        //trigger arm animation
        
        //rotates arms up:
        armL.localEulerAngles += new Vector3(-20, 0, 0);
        armR.localEulerAngles += new Vector3(-20, 0, 0);

        //moves the arms backward
        armL.position += -armL.transform.forward * 0.1f;
        armR.position += -armR.transform.forward * 0.1f;

    }

    private bool CanSeeThing(Transform thing)
    {
        if (!thing) return false;
        Vector3 vToThing = thing.position - transform.position;

        // check distance
        if (vToThing.sqrMagnitude > visionDis * visionDis) return false; // too far away to see

        // check direction
        if(Vector3.Angle(transform.forward, vToThing) > visionAngle) return false; //out of vision "cone"

        // occlusion check

        return true;
    }

    private void ScanForTargets()
    {
        cooldownScan = 1;

        potientalTargets.Clear();

        ShootableThing[] things = GameObject.FindObjectsOfType<ShootableThing>();

        foreach (ShootableThing thing in things)
        {
            if (CanSeeThing(thing.transform)) potientalTargets.Add(thing);
        }
    }

    void PickATarget()
    {

        cooldownPick = .25f;

        //if (target) return; // we have a target
        target = null;

        float cloestDistanceSoFar = 0;

        //finds closest target
        foreach(ShootableThing pt in potientalTargets)
        {
            float dd = (pt.transform.position - transform.position).sqrMagnitude;

            if(dd < cloestDistanceSoFar || target == null)
            {
                target = pt.transform;
                cloestDistanceSoFar = dd;
            }

        }
    }
}
