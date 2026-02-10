using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [SerializeField] GameObject gameOverUI;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // تضمینی: اول بازی خاموش
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
        else
            Debug.LogError("[GameOverManager] gameOverUI is NOT assigned!");
    }

    public void GameOver()
    {
        Debug.Log("[GameOver] GAME OVER ☠");

        var spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
            spawner.enabled = false;

        // اول UI بعد توقف زمان
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }
}
