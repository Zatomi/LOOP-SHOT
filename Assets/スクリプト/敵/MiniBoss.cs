using UnityEngine;

public class MiniBoss : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    public int maxHP = 3;
    private int currentHP;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("攻撃")]
    public GameObject enemyBulletPrefab;
    public float shootInterval = 3f;
    public float bulletSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        currentHP = maxHP;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグのオブジェクトが見つかりません");
        }

        if (animator == null)
        {
            Debug.LogWarning("中ボスのAnimatorが見つかりません");
        }

        if (spriteRenderer == null)
        {
            Debug.LogWarning("中ボスのSpriteRendererが見つかりません");
        }

        InvokeRepeating(nameof(Shoot), 2f, shootInterval);
    }

    void FixedUpdate()
    {
        if (player == null || rb == null)
            return;

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        // プレイヤーの左右位置に合わせて向きを変える
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void TakeDamage()
    {
        TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log("中ボスがダメージを受けた！ 残りHP：" + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Shoot()
    {
        if (player == null || enemyBulletPrefab == null)
            return;

        // 攻撃モーションを再生
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Vector2 direction =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        // 正面
        CreateBullet(direction);

        // 左側
        Vector2 leftDirection =
            Quaternion.Euler(0, 0, 20f) * direction;

        CreateBullet(leftDirection);

        // 右側
        Vector2 rightDirection =
            Quaternion.Euler(0, 0, -20f) * direction;

        CreateBullet(rightDirection);
    }

    void CreateBullet(Vector2 direction)
    {
        Vector2 spawnPosition =
            (Vector2)transform.position + direction * 0.8f;

        GameObject bullet =
            Instantiate(
                enemyBulletPrefab,
                spawnPosition,
                Quaternion.identity
            );

        Rigidbody2D bulletRb =
            bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity =
                direction * bulletSpeed;
        }
    }

    void Die()
    {
        EnemyDrop drop = GetComponent<EnemyDrop>();

        if (drop != null)
        {
            drop.DropBossItems();
        }

        Destroy(gameObject);
    }

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