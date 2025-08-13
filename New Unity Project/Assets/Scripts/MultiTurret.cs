using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTurret : Turret
{
    private void Start()
    {
        StartCoroutine(DamageEnemies());
    }

    new private void Update()
    {
        
    }

    private IEnumerator DamageEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= shootingDistance)
            {
                enemy.Health--;
            }
        }

        yield return new WaitForSeconds(shootingSpeed);

        StartCoroutine(DamageEnemies());
    }
}
