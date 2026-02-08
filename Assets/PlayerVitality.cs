using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerVitality : MonoBehaviour
{
    [Header("Drain (HP per second)")]
    [SerializeField] float drainPerSecond = 1f;

    [Header("Regen (HP per second)")]
    [SerializeField] float regenPerSecond = 6f;

    [Header("Controls")]
    [SerializeField] bool regenEnabled = true;

    Health health;
    float drainAcc;
    float regenAcc;

    void Awake()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (health.IsDead) return;

        // --- Drain
        drainAcc += drainPerSecond * Time.deltaTime;
        if (drainAcc >= 1f)
        {
            int dmg = Mathf.FloorToInt(drainAcc);
            drainAcc -= dmg;
            health.TakeDamage(dmg);
        }

        // --- Regen
        if (!regenEnabled) return;
        if (health.CurrentHP >= health.MaxHP) return;

        regenAcc += regenPerSecond * Time.deltaTime;
        if (regenAcc >= 1f)
        {
            int heal = Mathf.FloorToInt(regenAcc);
            regenAcc -= heal;
            health.Heal(heal);
        }
    }

    public void SetRegenEnabled(bool enabled)
    {
        regenEnabled = enabled;
        regenAcc = 0f;
    }
}
