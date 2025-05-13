using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage = 10f;
    public float damageCooldown = 1f;

    private float lastDamageTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time < lastDamageTime + damageCooldown) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.PlayerTakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}

