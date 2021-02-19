using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    private CharacterController cc;
    Vector3 inputDirection = new Vector3();

    private bool isTryingToMove = false;
    public Transform leg1;
    public Transform leg2;

    public float walkSpeed = 5;

    private float verticalVelocity = 0;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        WiggleLegs();
    }

    private void WiggleLegs()
    {
        float degrees = 45;
        float speed = 10;


        if (isTryingToMove)
        {
            Vector3 localDirection = transform.InverseTransformDirection(inputDirection);
            Vector3 axis = Vector3.Cross(localDirection, Vector3.up);

            //check localDirection against forward vector

            float alignment = Vector3.Dot(localDirection, Vector3.forward);

            //if (alignment < 0) alignment *= -1; // flip negative numbers
            alignment = Mathf.Abs(alignment); // flip negative numbers

            degrees *= AnimMath.Lerp(0.25f, 1, alignment); //decrease degree variable when strafing

            float wave = Mathf.Sin(Time.time * speed) * degrees; //  (-45 to 45)

            leg1.localRotation = AnimMath.Slide(leg1.localRotation, Quaternion.AngleAxis(wave, axis), 0.01f);
            leg2.localRotation = AnimMath.Slide(leg2.localRotation, Quaternion.AngleAxis(-wave, axis), 0.01f);
        }
        else
        {
            leg1.localRotation = AnimMath.Slide(leg1.localRotation, Quaternion.identity, 0.01f);
            leg2.localRotation = AnimMath.Slide(leg2.localRotation, Quaternion.identity, 0.01f);
        }
        
    }

    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal"); // strafing?
        float v = Input.GetAxis("Vertical"); // forward / backward

        isTryingToMove = (h != 0 || v != 0);
        if (isTryingToMove)
        {
            //turn to face correct direction
            float camYaw = cam.transform.eulerAngles.y;
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(0, camYaw, 0), 0.02f);
        }

        inputDirection = transform.forward * v + transform.right * h;

        //applying gravity
        if(!cc.isGrounded) verticalVelocity += 10 * Time.deltaTime;
        if (cc.isGrounded) verticalVelocity = 0;

        // adds lateral movement to vertical movement
        Vector3 moveDelta = inputDirection * walkSpeed + verticalVelocity * Vector3.down;

        //passes it all to the cc
        cc.Move(moveDelta * Time.deltaTime);
    }
}
