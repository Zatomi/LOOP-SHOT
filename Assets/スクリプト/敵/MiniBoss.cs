using UnityEngine;

public class MiniBoss : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    public int maxHP = 3;
    private int currentHP;

    private Transform player;
    private Rigidbody2D rb;

    [Header("攻撃")]
    public GameObject enemyBulletPrefab;
    public float shootInterval = 3f;
    public float bulletSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        InvokeRepeating(nameof(Shoot), 2f, shootInterval);
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        // プレイヤーを追いかける
        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }


    public void TakeDamage()
    {
        TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log("中ボスにダメージ！ 残りHP：" + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }


    void Shoot()
    {
        if (player == null || enemyBulletPrefab == null)
            return;

        // プレイヤーへの方向
        Vector2 direction =
            ((Vector2)player.position - (Vector2)transform.position).normalized;

        // 真ん中
        CreateBullet(direction);

        // 左
        Vector2 leftDirection =
            Quaternion.Euler(0, 0, 20f) * direction;

        CreateBullet(leftDirection);

        // 右
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
        // 回復アイテムを1個
        GameObject healItem =
            GameObject.Find("HealItem");

        // 特殊弾を2個
        GameObject specialItem =
            GameObject.Find("SpecialBulletItem");

        // EnemyDropから確実にドロップ
        EnemyDrop drop =
            GetComponent<EnemyDrop>();

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