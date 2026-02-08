using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject enemyPrefab;

    [Header("Spawn Points Parent")]
    [SerializeField] Transform enemySpawnPointsRoot;

    [Header("Wave Settings")]
    [SerializeField] int baseEnemyCount = 4;
    [SerializeField] int addPerWave = 2;
    [SerializeField] float spawnGap = 0.2f;
    [SerializeField] float spawnJitterRadius = 0.4f;

    [Header("Between Waves")]
    [SerializeField] bool waitForFullHealthToStartNextWave = true;

    int wave = 0;
    int aliveEnemies = 0;

    bool waveRunning = false;
    bool waitingForNextWave = false;

    Health playerHealth;
    PlayerVitality playerVitality;

    void Start()
    {
        if (enemyPrefab == null || enemySpawnPointsRoot == null)
        {
            Debug.LogError("EnemySpawner: Assign enemyPrefab and enemySpawnPointsRoot in Inspector.");
            enabled = false;
            return;
        }

        StartCoroutine(Bootstrap());
    }

    IEnumerator Bootstrap()
    {
        // صبر کن تا Player واقعاً Spawn شود
        while (!TryBindPlayer())
            yield return null;

        BeginWave();
    }

    bool TryBindPlayer()
    {
        if (playerHealth != null) return true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null) return false;

        playerHealth = p.GetComponent<Health>();
        playerVitality = p.GetComponent<PlayerVitality>();

        if (playerHealth == null)
        {
            Debug.LogError("EnemySpawner: Player has no Health component!");
            return false;
        }

        return true;
    }

    void Update()
    {
        // اگر Player به هر دلیل Destroy/Respawn شد
        if (playerHealth == null)
        {
            TryBindPlayer();
            return;
        }

        // بین موج‌ها: منتظر پر شدن HP باش
        if (waitingForNextWave && waitForFullHealthToStartNextWave)
        {
            // اگر اسم فیلدهای Health شما فرق داره، اینجا رو مطابق پروژه‌ات تغییر بده:
            if (playerHealth.CurrentHP >= playerHealth.MaxHP)
            {
                Debug.Log("[Spawner] Player full HP ✅ -> starting next wave");
                BeginWave();
            }
        }
    }

    void BeginWave()
    {
        wave++;
        waveRunning = true;
        waitingForNextWave = false;

        // هنگام موج ریکاوری خاموش
        if (playerVitality != null)
            playerVitality.SetRegenEnabled(false);

        int count = baseEnemyCount + (wave - 1) * addPerWave;
        Debug.Log($"[Spawner] Wave {wave} -> spawning {count} enemies");

        StopAllCoroutines();
        StartCoroutine(SpawnWave(count));
    }

    IEnumerator SpawnWave(int count)
    {
        aliveEnemies = 0;

        for (int i = 0; i < count; i++)
        {
            SpawnOneEnemy();
            yield return new WaitForSeconds(spawnGap);
        }
    }

    void SpawnOneEnemy()
    {
        int spCount = enemySpawnPointsRoot.childCount;
        if (spCount == 0)
        {
            Debug.LogError("EnemySpawner: No enemy spawn points under enemySpawnPointsRoot.");
            return;
        }

        int idx = Random.Range(0, spCount);
        Transform sp = enemySpawnPointsRoot.GetChild(idx);

        Vector2 pos = sp.position;
        pos += Random.insideUnitCircle * spawnJitterRadius;

        GameObject e = Instantiate(enemyPrefab, pos, Quaternion.identity);
        aliveEnemies++;

        // ضد-باگ: هر وقت این Enemy نابود شد (هر علتی)، شمارنده کم شود
        var tracker = e.GetComponent<EnemyDestroyTracker>();
        if (tracker == null) tracker = e.AddComponent<EnemyDestroyTracker>();

        tracker.onDestroyed += () =>
        {
            aliveEnemies--;
            Debug.Log($"[Spawner] Enemy destroyed -> aliveEnemies={aliveEnemies}");
            CheckWaveDone();
        };
    }

    void CheckWaveDone()
    {
        Debug.Log($"[Spawner] CheckWaveDone aliveEnemies={aliveEnemies} waveRunning={waveRunning}");

        if (!waveRunning) return;

        if (aliveEnemies <= 0)
        {
            waveRunning = false;

            Debug.Log($"[Spawner] Wave {wave} cleared ✅");

            // بین موج‌ها ریکاوری روشن
            if (playerVitality != null)
                playerVitality.SetRegenEnabled(true);

            // اگر نمی‌خوای منتظر full health باشی، همینجا موج بعدی رو شروع کن
            if (!waitForFullHealthToStartNextWave)
            {
                BeginWave();
            }
            else
            {
                Debug.Log("[Spawner] Waiting for full HP to start next wave...");
                waitingForNextWave = true;
            }
        }
    }
}

/// <summary>
/// هر وقت گیم‌اوبجکت Destroy شود، خبر می‌دهد.
/// این باعث می‌شود شمارنده‌ی aliveEnemies همیشه درست بماند.
/// </summary>
public class EnemyDestroyTracker : MonoBehaviour
{
    public System.Action onDestroyed;

    void OnDestroy()
    {
        onDestroyed?.Invoke();
    }
}
