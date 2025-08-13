using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private const int BASE_CUBES_NUMBER = 1;
    private const int BASE_SMALL_CUBES_NUMBER = 1;

    [SerializeField] private Enemy enemyCubePrefab = null;
    private int cubesNumber = 2;
    [SerializeField] private Enemy smallEnemyCubePrefab = null;
    private int smallCubesNumber = 1;
    private GameController gameController;
    private float respawnTime = 1f;
    [SerializeField] private Enemy bigEnemyCubePrefab = null;
    private int bigCubesNumber = 0;
    [SerializeField] private Enemy bossEnemyCubePrefab = null;
    private int bossCubesNumber = 0;
    [SerializeField] private Enemy smallBossEnemyCubePrefab = null;
    private int smallBossCubesNumber = 0;
    [SerializeField] private Enemy splitEnemyPrefab = null;
    private int splitEnemyNumber = 0;
    [SerializeField] private Enemy bigSplitEnemyPrefab = null;
    private int bigSplitEnemyNumber = 0;


    public int CubesNumber { get => cubesNumber; set => cubesNumber = value; }
    public int SmallCubesNumber { get => smallCubesNumber; set => smallCubesNumber = value; }
    public float RespawnTime { get => respawnTime; set => respawnTime = value; }

    public static int BaseCubesNumber => BASE_CUBES_NUMBER;
    public static int BaseSmallCubesNumber => BASE_SMALL_CUBES_NUMBER;

    public int BigCubesNumber { get => bigCubesNumber; set => bigCubesNumber = value; }
    public int BossCubesNumber { get => bossCubesNumber; set => bossCubesNumber = value; }
    public int SmallBossCubesNumber { get => smallBossCubesNumber; set => smallBossCubesNumber = value; }
    public int SplitEnemyNumber { get => splitEnemyNumber; set => splitEnemyNumber = value; }
    public int BigSplitEnemyNumber { get => bigSplitEnemyNumber; set => bigSplitEnemyNumber = value; }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void StartTurn()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(RespawnTime);
        if(SmallCubesNumber > 0)
        {
            Instantiate(smallEnemyCubePrefab, transform.position, Quaternion.Euler(0f, 180f, 0f));
            SmallCubesNumber--;
            StartCoroutine(SpawnEnemy());
        }
        else if (SmallBossCubesNumber > 0)
        {
            Instantiate(smallBossEnemyCubePrefab, transform.position, Quaternion.Euler(0f, 180f, 0f));
            SmallBossCubesNumber--;
            StartCoroutine(SpawnEnemy());
        }
        else if (CubesNumber > 0)
        {
            Instantiate(enemyCubePrefab, transform.position, Quaternion.Euler(0f, 180f, 0f));
            CubesNumber--;
            StartCoroutine(SpawnEnemy());
        }
        else if (BigCubesNumber > 0)
        {
            Instantiate(bigEnemyCubePrefab, transform.position, Quaternion.Euler(0f, 180f, 0f));
            BigCubesNumber--;
            StartCoroutine(SpawnEnemy());
        }
        else if (SplitEnemyNumber > 0)
        {
            Instantiate(splitEnemyPrefab, transform.position, Quaternion.Euler(0f, 180f, 0f));
            SplitEnemyNumber--;
            StartCoroutine(SpawnEnemy());
        }
        else if (BigSplitEnemyNumber > 0)
        {
            Instantiate(bigSplitEnemyPrefab, transform.position, Quaternion.Euler(0f, 180f, 0f));
            BigSplitEnemyNumber--;
            StartCoroutine(SpawnEnemy());
        }
        else if (BossCubesNumber > 0)
        {
            Instantiate(bossEnemyCubePrefab, transform.position, Quaternion.Euler(0f, 180f, 0f));
            BossCubesNumber--;
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            gameController.NextTurn();
        }
    }
}
