using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] Turret turretCubePrefab = null;
    [SerializeField] Turret bigTurretCubePrefab = null;
    [SerializeField] Turret slowTurretPrefab = null;
    [SerializeField] Turret multiTurretPrefab = null;

    private GameController gameController;
        
    private BuildArea buildArea;

    public BuildArea BuildArea { get => buildArea; set => buildArea = value; }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void PlaceTurret(Turret turretPrefab)
    {
        if(!buildArea.Occupied && gameController.Money >= turretPrefab.Cost)
        {
            Vector3 pos = new Vector3(BuildArea.transform.position.x, turretCubePrefab.transform.localScale.y, BuildArea.transform.position.z);

            BuildArea.BuiltTurret = Instantiate(turretPrefab, pos, Quaternion.identity);
            BuildArea.Occupied = true;

            gameController.Money -= turretPrefab.Cost;
        }
    }

    public void PlaceTurretCube()
    {
        PlaceTurret(turretCubePrefab);
    }

    public void PlaceBigTurretCube()
    {
        PlaceTurret(bigTurretCubePrefab);
    }

    public void PlaceSlowTurret()
    {
        PlaceTurret(slowTurretPrefab);
    }

    public void PlaceMultiTurret()
    {
        PlaceTurret(multiTurretPrefab);
    }

    public void DestroyTurret()
    {
        if(buildArea.Occupied)
        {
            Destroy(buildArea.BuiltTurret.gameObject);
            buildArea.Occupied = false;
        }
    }
}
