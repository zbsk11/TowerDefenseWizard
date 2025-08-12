using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FortressHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    public int Current { get; private set; }

    public event Action OnFortressDestroyed;

    private void Awake() => Current = maxHealth;

    public void TakeDamage(int amount)
    {
        Current -= amount;
        if (Current <= 0)
        {
            Current = 0;
            OnFortressDestroyed?.Invoke();
            Debug.Log("Fortress destroyed — GAME OVER");
        }
    }
}