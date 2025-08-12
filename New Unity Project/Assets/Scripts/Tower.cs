using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] float range = 6f;
    [SerializeField] float fireRate = 1.5f;   // shots per second
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    float cooldown;

    void Awake()
    {
        if (!firePoint)
        {
            var go = new GameObject("FirePoint");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0, 1.2f, 0);
            firePoint = go.transform;
        }
    }

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0f) return;

        Transform target = FindTarget();
        if (target)
        {
            Shoot(target);
            cooldown = 1f / fireRate;
        }
    }

    Transform FindTarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform best = null; float bestDist = Mathf.Infinity;
        Vector3 p = transform.position;

        foreach (var e in enemies)
        {
            float d = Vector3.Distance(p, e.transform.position);
            if (d < range && d < bestDist) { best = e.transform; bestDist = d; }
        }
        return best;
    }

    void Shoot(Transform target)
    {
        var go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        go.GetComponent<Projectile>().Seek(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
