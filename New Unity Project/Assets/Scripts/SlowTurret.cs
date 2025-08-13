using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTurret : Turret
{
    new private void Update()
    {
        SlowEnemies();
    }

    private void SlowEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach(Enemy enemy in enemies)
        {
            if(!enemy.SpeedDivided && Vector3.Distance(transform.position, enemy.transform.position) <= shootingDistance)
            {
                StartCoroutine(enemy.DivideSpeed(2));
            }
        }
    }
}
