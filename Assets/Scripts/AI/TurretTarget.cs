using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTarget : MonoBehaviour
{

    public Transform Player;

    private bool isClose = false;

    Quaternion lookingRotation;
    Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceCheck();


        if (!isClose)
        {
            lookingRotation = Quaternion.Euler(0, 0, 0);
            transform.localRotation = AnimMath.Slide(transform.localRotation, lookingRotation, 0.01f);
        }
        else
        {
            Vector3 disToTarget = Player.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(disToTarget, Vector3.up);

            transform.localRotation = AnimMath.Slide(transform.localRotation, targetRotation, 0.001f);
        }

    }

    private void DistanceCheck()
    {
        float disCheck = Vector3.Distance(transform.position, Player.position);

        if (disCheck <= 10)
        {
            isClose = true;
        }
        else
        {
            isClose = false;
        }
    }
}
