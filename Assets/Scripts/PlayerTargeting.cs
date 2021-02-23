using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public List<ShootableThing> potientalTargets = new List<ShootableThing>();

    public Transform target;
    public bool wantsToTarget = false;

    public float visionDis = 10;

    float cooldownScan = 0;

    float cooldownPick = 0;
    public float visionAngle = 80;

    void Start()
    {
        //set this at end of project it's annoying while developing
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!wantsToTarget) target = null;

        wantsToTarget = Input.GetButton("Fire2");

        cooldownScan -= Time.deltaTime; // counting down

        if (cooldownScan <= 0 || target == null && wantsToTarget) ScanForTargets(); // do this when count down finished

        cooldownPick -= Time.deltaTime;

        if (cooldownPick <= 0) PickATarget();


        if (target && !CanSeeThing(target)) target = null;
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
