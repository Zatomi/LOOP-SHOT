using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    // プレイヤーに触れている間、ダメージ判定
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeHit();
            }
        }
    }
}