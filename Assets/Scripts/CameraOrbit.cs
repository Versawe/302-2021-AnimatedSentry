using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{

    public PlayerMovement moveScript;
    private PlayerTargeting targetScript;
    private Camera cam;

    private float yaw = 0;
    private float pitch = 0;

    public float cameraSensitivityX = 10;
    public float cameraSensitivityY = 10;

    public float shakeIntensity = 0;

    // Start is called before the first frame update
    void Start()
    {
        targetScript = moveScript.GetComponent<PlayerTargeting>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerOrbitCamera();

        transform.position = moveScript.transform.position;


        //if aiming set camera rotation to look at target
        RotateCamToLookAtTarget();

        //"zoom" in the camera
        ZoomCamera();

        ShakeCamera();
    }

    public void Shake(float intensity = 1)
    {
        if(intensity > 1)
        {
            shakeIntensity = intensity;
        }
        else
        {
            shakeIntensity += intensity;
            if (shakeIntensity > 1) shakeIntensity = 1;
        }

    }

    private void ShakeCamera()
    {
        if (shakeIntensity < 0) shakeIntensity = 0;
        if (shakeIntensity > 0) shakeIntensity -= Time.deltaTime;
        else return; // shake intensity is 0 so don't do math
        //pick a small random rotation
        Quaternion targetRot = AnimMath.Lerp(Random.rotation, Quaternion.identity, .996f);

        //cam.transform.localRotation *= targetRot;
        cam.transform.localRotation = AnimMath.Lerp(cam.transform.localRotation, cam.transform.localRotation * targetRot, shakeIntensity * shakeIntensity);
    }

    private void ZoomCamera()
    {
        float dis = 6;

        if (targetScript && targetScript.target != null && targetScript.wantsToTarget) dis = 3;

        cam.transform.localPosition = AnimMath.Slide(cam.transform.localPosition, new Vector3(0,0,-dis), .001f);

      
    }

    private void RotateCamToLookAtTarget()
    {
        //if targeting look at target
        if(targetScript && targetScript.target != null && targetScript.wantsToTarget)
        {
            Vector3 lookTarget = targetScript.target.position - cam.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(lookTarget, Vector3.up);

            cam.transform.rotation = AnimMath.Slide(cam.transform.rotation, targetRotation, .001f);
        }
        else
        {
            //if not
            cam.transform.localRotation = AnimMath.Slide(cam.transform.localRotation, Quaternion.identity, .001f);  // no rotation....
        }

    }

    private void PlayerOrbitCamera()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        yaw += mx * cameraSensitivityX;
        pitch += my * cameraSensitivityY;

        if (targetScript && targetScript.target != null && targetScript.wantsToTarget)
        {
            pitch = Mathf.Clamp(pitch, 15, 60);

            //find player facing
            float playerYaw = moveScript.transform.eulerAngles.y;
            //clamp camera-rig Yaw to playerYaw +- 40
            yaw = Mathf.Clamp(yaw, playerYaw - 40, playerYaw + 40);
        }
        else
        {
            pitch = Mathf.Clamp(pitch, -10, 89);
        }

        transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .001f);
    }
}
