using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] protected Bullet bulletPrefab = null;
    [SerializeField] protected float shootingSpeed = 1f;
    [SerializeField] protected float shootingDistance = 10f;
    [SerializeField] private int cost = 25;

    protected bool bulletShot = false;

    public int Cost { get => cost; }

    protected void Update()
    {
        Enemy target = GetEnemyInDistance();

        if(!bulletShot && target != null)
        {
            StartCoroutine(ShootBullet(target));
        }
    }

    protected virtual IEnumerator ShootBullet(Enemy target)
    {
        bulletShot = true;
        Bullet bullet = Instantiate(bulletPrefab, transform.position,Quaternion.identity);
        bullet.Target = target;
        yield return new WaitForSeconds(shootingSpeed);
        bulletShot = false;
    }

    private Enemy GetEnemyInDistance()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach(Enemy enemy in enemies)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) <= shootingDistance)
            {
                return enemy;
            }
        }

        return null;
    }
}
