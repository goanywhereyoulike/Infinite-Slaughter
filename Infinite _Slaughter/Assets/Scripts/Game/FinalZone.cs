using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZone : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        DestructibleObject enemy = other.gameObject.GetComponent<DestructibleObject>();
        if (enemy != null)
        {
            enemy.CurrentHealth = 0;
            player.gameObject.GetComponent<DestructibleObject>().TakeDamage(10);
            ServiceLocator.Get<GameManager>().UpdateHealthBar(player.gameObject.GetComponent<DestructibleObject>().CurrentHealth);

        }
    }
}
