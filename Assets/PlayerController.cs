using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;

    Rigidbody2D rb;
    PlayerInput input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Vector2 dir = input.Move;

        if (dir.sqrMagnitude > 1f)
            dir = dir.normalized;

        rb.linearVelocity = dir * moveSpeed;
    }
}
