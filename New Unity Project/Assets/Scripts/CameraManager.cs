using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum Mode { Overview, Fortress, TowerView, EnemyPOV }

    [Header("Refs")]
    [SerializeField] private Camera cam;

    [Header("Anchors")]
    [SerializeField] private Transform overviewAnchor;
    [SerializeField] private Transform fortressAnchor;

    [Header("Motion")]
    [SerializeField] private float moveLerp = 7f;
    [SerializeField] private float rotLerp  = 7f;

    private readonly List<Transform> towerViews = new();
    private Mode mode = Mode.Overview;
    private Transform targetAnchor;       // where we fly to (for non-POV modes)
    private Transform enemyFollow;        // enemy head transform (POV)
    private Vector3 enemyLocalPos = new(0f, 1.6f, 0.1f); // eye height/offset
    private Quaternion enemyLocalRot = Quaternion.identity;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        RefreshTowerViews();
        SetOverview();
    }

    void Update()
    {
        // Rebuild tower view list if count changed (towers placed)
        // Cheap check each second; fine for now
        if (Time.frameCount % 60 == 0) RefreshTowerViews();

        if (mode == Mode.EnemyPOV)
        {
            if (!enemyFollow)
            {
                // Try to acquire another enemy
                AcquireEnemy();
                if (!enemyFollow) { SetOverview(); return; }
            }
            // stick to enemy 
            cam.transform.position = enemyFollow.TransformPoint(enemyLocalPos);
            cam.transform.rotation = enemyFollow.rotation * enemyLocalRot;
        }
        else
        {
            if (!targetAnchor) return;
            cam.transform.position = Vector3.Lerp(
                cam.transform.position,
                targetAnchor.position,
                1f - Mathf.Exp(-moveLerp * Time.deltaTime)
            );
            cam.transform.rotation = Quaternion.Slerp(
                cam.transform.rotation,
                targetAnchor.rotation,
                1f - Mathf.Exp(-rotLerp * Time.deltaTime)
            );
        }
    }

    // ---------- Public API ----------
    public void SetOverview()
    {
        mode = Mode.Overview;
        targetAnchor = overviewAnchor;
        enemyFollow = null;
    }

    public void SetFortress()
    {
        mode = Mode.Fortress;
        targetAnchor = fortressAnchor;
        enemyFollow = null;
    }

    public void SetTowerIndex(int i)
    {
        if (i < 0 || i >= towerViews.Count) { Debug.LogWarning("No such tower view"); return; }
        mode = Mode.TowerView;
        targetAnchor = towerViews[i];
        enemyFollow = null;
    }

    public void SetEnemyPOV()
    {
        mode = Mode.EnemyPOV;
        targetAnchor = null;
        AcquireEnemy();
    }

    // ---------- Helpers ----------
    private void AcquireEnemy()
    {
        enemyFollow = null;
        var all = GameObject.FindGameObjectsWithTag("Enemy");
        if (all.Length == 0) return;

        // pick the first alive
        enemyFollow = all[0].transform;
    }

    private void RefreshTowerViews()
    {
        var towers = GameObject.FindGameObjectsWithTag("Tower");
        towerViews.Clear();
        foreach (var t in towers)
        {
            Transform vp = t.transform.Find("ViewPoint");
            if (!vp)
            {
                // create a simple temporary anchor beside the tower
                var temp = new GameObject("TempViewPoint").transform;
                temp.SetParent(t.transform, worldPositionStays: false);
                temp.localPosition = new Vector3(0, 6, -7);
                temp.localRotation = Quaternion.identity;
                vp = temp;
            }
            towerViews.Add(vp);
        }
    }
}
