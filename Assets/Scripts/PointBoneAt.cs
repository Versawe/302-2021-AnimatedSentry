using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBoneAt : MonoBehaviour
{

    private PlayerTargeting pt;

    public Vector3 naturalAimDirection;

    private Quaternion startingRotation;

    public bool lockRotationX;
    public bool lockRotationY;
    public bool lockRotationZ;

    // Start is called before the first frame update
    void Start()
    {
        startingRotation = transform.localRotation;
        pt = GetComponentInParent<PlayerTargeting>();
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardsTarget();
    }

    private void TurnTowardsTarget()
    {
        if (pt && pt.target && pt.wantsToTarget) {
            Vector3 disToTarget = pt.target.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(disToTarget, Vector3.up);

            Vector3 euler1 = transform.localEulerAngles; // get local angle before rotation

            Quaternion prevRot = transform.rotation;
            transform.rotation = targetRotation; // set rotation
            Vector3 euler2 = transform.localEulerAngles; // get local angle after rotation

            if (lockRotationX) euler2.x = euler1.x; // reverts
            if (lockRotationY) euler2.y = euler1.y;
            if (lockRotationZ) euler2.z = euler1.z;

            transform.rotation = prevRot; // revert rotation

            //animate rotation
            transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(euler2), .001f);
        }
        else
        {
            // figure out bone rotation, no target:

            transform.localRotation = AnimMath.Slide(transform.localRotation, startingRotation, .05f);
        }
        
    }
}
