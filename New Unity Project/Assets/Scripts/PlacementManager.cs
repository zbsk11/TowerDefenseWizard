using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject towerPrefab;

    [Header("Economy")]
    [SerializeField] private int towerCost = 20;

    [Header("Masks")]
    [SerializeField] private LayerMask groundMask;   // where we can click
    [SerializeField] private LayerMask blockedMask;  // Towers / Path layers

    [Header("Placement")]
    [SerializeField] private float gridSize = 1f;      // snap to grid
    [SerializeField] private float blockRadius = 0.6f; // overlap check

    private void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryPlace();
    }

    private void TryPlace()
    {
        if (!towerPrefab) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
            return;

        Vector3 p = SnapToGrid(hit.point);

        // prevent placing on top of other towers / blocked areas
        if (Physics.CheckSphere(p + Vector3.up * 0.5f, blockRadius, blockedMask))
            return;

        // spend points (or place freely if no GameManager yet)
        if (GameManager.I == null || GameManager.I.Spend(towerCost))
        {
            Instantiate(towerPrefab, p + Vector3.up * 1f, Quaternion.identity);
        }
        else
        {
            Debug.Log("Not enough points.");
        }
    }

    private Vector3 SnapToGrid(Vector3 p)
    {
        p.x = Mathf.Round(p.x / gridSize) * gridSize;
        p.z = Mathf.Round(p.z / gridSize) * gridSize;
        return new Vector3(p.x, 0f, p.z);
    }
}
