using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI pointsText;

    [Header("Game References")]
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private FortressHealth fortressHealth;

    private int lastPoints = int.MinValue;

    private void Awake()
    {
        if (!waveSpawner)    waveSpawner    = FindObjectOfType<WaveSpawner>();
        if (!fortressHealth) fortressHealth = FindObjectOfType<FortressHealth>();
    }

    private void OnEnable()
    {
        if (GameManager.I != null)
            GameManager.I.OnPointsChanged += UpdatePointsUI;
    }

    private void OnDisable()
    {
        if (GameManager.I != null)
            GameManager.I.OnPointsChanged -= UpdatePointsUI;
    }

    private void Start()
    {
        UpdateWaveUI();
        UpdateHealthUI();
        if (GameManager.I) UpdatePointsUI(GameManager.I.Points);
    }

    private void Update()
    {
        // Always keep these fresh
        UpdateWaveUI();
        UpdateHealthUI();

        // Poll points in case we missed the event due to init order
        if (GameManager.I && GameManager.I.Points != lastPoints)
            UpdatePointsUI(GameManager.I.Points);
    }

    private void UpdateWaveUI()
    {
        if (waveText && waveSpawner)
            waveText.text = $"Wave: {waveSpawner.CurrentWave + 1} / {waveSpawner.TotalWaves}";
    }

    private void UpdateHealthUI()
    {
        if (healthText && fortressHealth)
            healthText.text = $"Health: {fortressHealth.Current}";
    }

    private void UpdatePointsUI(int v)
    {
        lastPoints = v;
        if (pointsText) pointsText.text = $"Points: {v}";
    }
}
