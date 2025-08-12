using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int pointsOnDeath = 5;
    public int Current { get; private set; }

    private void Awake() => Current = maxHealth;

    public void TakeDamage(int amount)
    {
        Current -= amount;
        if (Current <= 0) Die();
    }

    private void Die()
    {
        if (GameManager.I) GameManager.I.AddPoints(pointsOnDeath);
        Destroy(gameObject);
    }
}
