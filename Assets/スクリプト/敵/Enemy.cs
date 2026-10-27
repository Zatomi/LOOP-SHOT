using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Transform player;
    private Rigidbody2D rb;

    // 元の大きさを保存する
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 最初の大きさを保存
        originalScale = transform.localScale;

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

        // 左右の移動方向に合わせて向きを変更
        if (direction.x > 0.01f)
        {
            // 右に移動するとき
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else if (direction.x < -0.01f)
        {
            // 左に移動するとき
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }

        // プレイヤーを追いかける
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
