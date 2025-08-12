using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Update()
    {
        if (!panel) return;
        panel.SetActive(Mathf.Approximately(Time.timeScale, 0f));
    }
}
