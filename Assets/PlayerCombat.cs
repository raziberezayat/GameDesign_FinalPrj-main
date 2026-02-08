using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField] KeyCode shootKey = KeyCode.Space;
    [SerializeField] float cooldown = 0.25f;
    [SerializeField] float range = 12f;
    [SerializeField] LayerMask enemyLayer;

    [Header("Aim")]
    [SerializeField] bool aimWithMouse = true;

    float timer;

    

    void Start()
    {
        if (enemyLayer.value == 0)
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        Debug.Log("[PlayerCombat] enemyLayer was 0, auto-set to Enemy ✅");
    }

        Debug.Log($"[PlayerCombat] Attached to: {gameObject.name} | enemyLayer.value={enemyLayer.value}");
        if (Camera.main == null)
            Debug.LogWarning("[PlayerCombat] Camera.main is NULL (check MainCamera tag)");
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetKeyDown(shootKey))
        {
            Debug.Log($"[PlayerCombat] KeyDown {shootKey} ✅ | on {gameObject.name}");
        }

        if (Input.GetKey(shootKey) && timer <= 0f)
        {
            timer = cooldown;
            Shoot();
        }
    }

    void Shoot()
    {
        Vector2 origin = transform.position;
        Vector2 dir = GetAimDirection(origin);

        // Draw the ray in Scene view (turn Gizmos on)
        Debug.DrawRay(origin, dir * range, Color.yellow, 0.4f);
        Debug.Log($"[PlayerCombat] Shoot origin={origin} dir={dir} range={range}");

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, range, enemyLayer);

        if (hit.collider == null)
        {
            Debug.Log("[PlayerCombat] Raycast: NO HIT ❌");
            return;
        }

        string hitLayer = LayerMask.LayerToName(hit.collider.gameObject.layer);
        Debug.Log($"[PlayerCombat] Raycast HIT ✅ name={hit.collider.name} layer={hitLayer}");

        // Because collider is on child, Health is on parent
        Health h = hit.collider.GetComponentInParent<Health>();
        Debug.Log("[PlayerCombat] Health found? " + (h != null));

        if (h != null)
        {
            Debug.Log("[PlayerCombat] Applying damage = 1");
            h.TakeDamage(1);
        }
    }

    Vector2 GetAimDirection(Vector2 origin)
    {
        if (!aimWithMouse)
            return Vector2.right;

        Camera cam = Camera.main;
        if (cam == null)
            return Vector2.right;

        // Correct mouse world point on z=0 plane
        float zDist = -cam.transform.position.z;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDist)
        );

        Vector2 dir = (Vector2)mouseWorld - origin;

        if (dir.sqrMagnitude < 0.0001f)
            return Vector2.right;

        return dir.normalized;
    }
}
