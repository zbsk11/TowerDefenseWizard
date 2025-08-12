using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public int count = 5;
        public float rate = 1f;
    }

    [Header("Refs")]
    [SerializeField] private WaypointPath path;
    [SerializeField] private FortressHealth fortress;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves = 5f;

    private int waveIndex = 0;

    // Properties for UI
    public int CurrentWave => waveIndex; // 0-based index for UI (+1 in display)
    public int TotalWaves => waves != null ? waves.Length : 0;

    private void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(2f); // short delay before first wave

        while (waveIndex < TotalWaves)
        {
            yield return StartCoroutine(SpawnWave(waves[waveIndex]));
            waveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        StartCoroutine(WatchForWin());
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(1f / Mathf.Max(0.01f, wave.rate));
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        var go = Instantiate(prefab, transform.position, Quaternion.identity);
        var mover = go.GetComponent<EnemyMovement>();
        if (mover != null) mover.Init(path, fortress);
    }

    private IEnumerator WatchForWin()
    {
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;

        Debug.Log("All waves cleared.");
    }
}
