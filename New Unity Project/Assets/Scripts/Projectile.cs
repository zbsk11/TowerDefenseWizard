using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float maxLife = 4f;

    private Transform target;

    // Called by Tower when firing
    public void Seek(Transform t)
    {
        target = t;
        // auto-despawn in case it never hits
        Destroy(gameObject, maxLife);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float step = speed * Time.deltaTime;

        // reached target this frame
        if (dir.magnitude <= step)
        {
            Hit();
            return;
        }

        transform.Translate(dir.normalized * step, Space.World);
    }

    private void Hit()
    {
        var hp = target.GetComponent<EnemyHealth>();
        if (hp != null)
            hp.TakeDamage(damage);

        Destroy(gameObject);
    }
}
