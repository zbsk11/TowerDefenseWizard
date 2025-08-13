using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : Enemy
{
    [SerializeField] private Enemy smallBossEnemyCubePrefab = null;
    private int spawnsNumber = 2;

    private void Update()
    {
        if (agent.remainingDistance < 2)
        {
            Destroy(gameObject);
            GameController.PlayerHealth--;
        }

        if (Health <= 0)
        {
            for(int i = 0; i < spawnsNumber; i++)
            {
                Instantiate(smallBossEnemyCubePrefab, transform.position, Quaternion.Euler(0f, 180f, 0f));
            }
            Destroy(gameObject);
        }
    }
}
