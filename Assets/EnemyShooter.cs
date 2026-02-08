using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] float shootInterval = 1.2f;
    [SerializeField] float range = 12f;
    [SerializeField] int damage = 5;

    float timer;
    Transform player;

    void Start()
    {
        timer = Random.Range(0f, shootInterval); // برای اینکه همه با هم شلیک نکنن
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            return;
        }

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = shootInterval;
        ShootRay();
    }

    void ShootRay()
    {
        Vector2 origin = transform.position;
        Vector2 dir = ((Vector2)player.position - origin).normalized;

        var hit = Physics2D.Raycast(origin, dir, range);
        if (hit.collider == null) return;

        // فقط اگر به Player خورد
        if (hit.collider.CompareTag("Player"))
        {
            var h = hit.collider.GetComponent<Health>();
            if (h != null) h.TakeDamage(damage);
        }
    }
}
