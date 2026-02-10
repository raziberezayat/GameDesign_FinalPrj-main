using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour
{
    Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += OnPlayerDeath;
    }

    void OnDestroy()
    {
        health.OnDeath -= OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        Debug.Log("[PlayerDeath] Player died ❌");

        // قطع کنترل‌ها
        DisableIfExists<PlayerController>();
        DisableIfExists<PlayerCombat>();
        DisableIfExists<PlayerInput>();
        DisableIfExists<PlayerVitality>();

        // توقف فیزیک
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // اعلام Game Over
        GameOverManager.Instance.GameOver();
    }

    void DisableIfExists<T>() where T : Behaviour
    {
        var c = GetComponent<T>();
        if (c != null) c.enabled = false;
    }
}
