using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] int damage = 5;
    [SerializeField] float damageInterval = 1f;

    float timer;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = damageInterval;

        var health = collision.collider.GetComponent<Health>();
        if (health != null && !health.IsDead)
        {
            health.TakeDamage(damage);
            Debug.Log("[Enemy] Contact damage → Player HP -" + damage);
        }
    }
}
