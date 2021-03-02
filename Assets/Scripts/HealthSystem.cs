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

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        health -= amount;

        if (health <= 0) Die();
    }

    public void Die()
    {
        //removes gameobject from the game
        //Destroy(gameObject);
        isDying = true;
    }
}
