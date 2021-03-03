using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health { get; private set; }

    public float healthMax = 100;

    public bool isDying = false;

    private void Start()
    {
        health = healthMax;
    }
    //apply damage to object with script attached
    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        health -= amount;

        if (health <= 0) Die();
    }
    //to let us know if object's health is less than or == 0
    public void Die()
    {
        isDying = true;
    }
}
