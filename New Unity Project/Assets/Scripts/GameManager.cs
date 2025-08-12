using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("Economy")]
    [SerializeField] private int startingPoints = 50;
    public int Points { get; private set; }
    public event Action<int> OnPointsChanged;

    [Header("Refs")]
    [SerializeField] private FortressHealth fortress;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        Points = startingPoints;
        OnPointsChanged?.Invoke(Points);

        if (!fortress) fortress = FindObjectOfType<FortressHealth>();
        if (fortress) fortress.OnFortressDestroyed += OnGameOver;
    }

    public void AddPoints(int amount)
    {
        Points += amount;
        OnPointsChanged?.Invoke(Points);
    }

    public bool Spend(int amount)
    {
        if (Points < amount) return false;
        Points -= amount;
        OnPointsChanged?.Invoke(Points);
        return true;
    }

    public void TogglePause()
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0f) ? 1f : 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void OnGameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("GAME OVER");
    }
}
