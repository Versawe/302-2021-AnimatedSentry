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

    void Start()
    {
        
    }

    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");

        cooldownScan -= Time.deltaTime; // counting down

        if (cooldownScan <= 0) ScanForTargets(); // do this when count down finished

        cooldownPick -= Time.deltaTime;

        if (cooldownPick <= 0) PickATarget();
    }

    private void ScanForTargets()
    {
        potientalTargets.Clear();

        cooldownScan = 1;

        ShootableThing[] things = GameObject.FindObjectsOfType<ShootableThing>();

        foreach (ShootableThing thing in things)
        {
            Vector3 disToThing = thing.transform.position - transform.position;

            if(disToThing.sqrMagnitude < visionDis * visionDis)
            {
                if(Vector3.Angle(transform.forward, disToThing) < 45)
                {
                    potientalTargets.Add(thing);
                }
            }

            //check direction


        }
    }

    void PickATarget()
    {

        cooldownPick = .25f;

        if (target) return; // we have a target

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
