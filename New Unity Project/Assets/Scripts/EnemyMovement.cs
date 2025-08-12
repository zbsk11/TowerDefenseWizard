using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damageToFortress = 1;

    private WaypointPath path;
    private FortressHealth fortress;
    private int index;

    // Used by the spawner to provide references & snap to WP0
    public void Init(WaypointPath p, FortressHealth f)
    {
        path = p;
        fortress = f;
        index = 0;

        if (path != null && path.Waypoints.Length > 0)
            transform.position = path.Waypoints[0].position;
    }

    private void Start()
    {
        if (path == null) path = FindObjectOfType<WaypointPath>();
        if (fortress == null) fortress = FindObjectOfType<FortressHealth>();

        index = 0;
        if (path != null && path.Waypoints.Length > 0)
            transform.position = path.Waypoints[0].position;
    }

    private void Update()
    {
        if (path == null || path.Waypoints == null || path.Waypoints.Length == 0)
            return;

        Transform target = path.Waypoints[index];
        Vector3 toTarget = target.position - transform.position;
        float step = speed * Time.deltaTime;

        if (toTarget.magnitude <= step)
        {
            transform.position = target.position;
            index++;

            // reached end -> damage fortress and die
            if (index >= path.Waypoints.Length)
            {
                if (fortress != null)
                    fortress.TakeDamage(damageToFortress);

                Destroy(gameObject);
                return;
            }
        }
        else
        {
            transform.Translate(toTarget.normalized * step, Space.World);
        }
    }
}
