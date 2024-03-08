using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int currentHealth;

    private bool isInvulnerable = false; // Flag to track invulnerability
    private float invulnerabilityDuration = 1f; // Duration of invulnerability in seconds

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bird") && !isInvulnerable) // Check if not already invulnerable
        {
            TakeDamage(10);
            Debug.Log("Player's health decreased. Current health: " + currentHealth);
            StartCoroutine(InvulnerabilityCooldown());
        }
    }

    private void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }

    // Coroutine for invulnerability cooldown
    private IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true;

        // Wait for the invulnerability duration
        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false; // Make player vulnerable again after cooldown
    }
}