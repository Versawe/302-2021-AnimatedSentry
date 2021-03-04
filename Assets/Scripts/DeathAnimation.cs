using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public Transform rightLeg;
    public Transform leftLeg;
    public Transform upperTorso;
    public Transform lowerTorso;
    public Transform hips;
    public Transform neck;
    public Transform head;

    HealthSystem playerHealth;

    public GameObject AudioPOBJ;

    // Start is called before the first frame update
    void Start()
    {
        //grabs script from player to know if player is dead or not
        playerHealth = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //trigger death animation if isDying on player is true
        if (!playerHealth.isDying) return; //if alive won't continue
        AudioPOBJ.SetActive(true);

        //Death Animation which is a variety of the player's joints sinking into the ground
        upperTorso.localPosition = AnimMath.Slide(upperTorso.localPosition, new Vector3(0, -1, 0), 0.5f);
        hips.localPosition = AnimMath.Slide(hips.localPosition, new Vector3(0, -1, 0), 0.5f);
        lowerTorso.localPosition = AnimMath.Slide(lowerTorso.localPosition, new Vector3(0, -1, 0), 0.5f);
        leftLeg.localPosition = AnimMath.Slide(leftLeg.localPosition, new Vector3(0, -1, 0), 0.5f);
        rightLeg.localPosition = AnimMath.Slide(rightLeg.localPosition, new Vector3(0, -1, 0), 0.5f);
        neck.localPosition = AnimMath.Slide(neck.localPosition, new Vector3(0, -0.5f, 0), 0.5f);
        head.localPosition = AnimMath.Slide(head.localPosition, new Vector3(0, -0.5f, 0), 0.5f);

    }
}
