using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    private PlayerController player;
    public float MaxHealth = 100.0f;

    private float currenthealth;
    public float CurrentHealth { get { return currenthealth; }set { currenthealth = value; } }

    private void Awake()
    {
        currenthealth = MaxHealth;
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        //if (currenthealth <= 0.0f)
        //{
        //    if (player != null)
        //    {
        //        return;
        //    }

        //    Destroy(gameObject);

        //}

    }
    public void TakeDamage(float damage)
    {
        currenthealth -= damage;

    }
}
