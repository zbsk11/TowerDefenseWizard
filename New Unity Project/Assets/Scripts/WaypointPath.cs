using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform[] Waypoints;

    private void OnValidate()
    {
        // Auto-fill with children, in hierarchy order
        int count = transform.childCount;
        Waypoints = new Transform[count];
        for (int i = 0; i < count; i++)
            Waypoints[i] = transform.GetChild(i);
    }

    private void OnDrawGizmos()
    {
        if (Waypoints == null || Waypoints.Length < 2) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < Waypoints.Length - 1; i++)
        {
            if (Waypoints[i] && Waypoints[i+1])
                Gizmos.DrawLine(Waypoints[i].position, Waypoints[i+1].position);
        }
    }
}
