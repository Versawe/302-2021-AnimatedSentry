using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    private CharacterController cc;

    public float walkSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        float h = Input.GetAxis("Horizontal"); // strafing?
        float v = Input.GetAxis("Vertical"); // forward / backward

        float yaw = Mathf.Atan2(v, h); //in radians
        float yawOfCamera = cam.transform.eulerAngles.y; // in degrees

        bool isTryingToMove = (h != 0 || v != 0);
        if (isTryingToMove)
        {
            //turn to face correct direction
            float camYaw = cam.transform.eulerAngles.y;
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(0, camYaw, 0), 0.02f);
        }

        Vector3 inputDirection = transform.forward * v +transform.right * h;

        cc.SimpleMove(inputDirection * walkSpeed);
    }
}
