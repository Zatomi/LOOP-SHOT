using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    public float explosionRadius = 2f;
    public GameObject explosionEffectPrefab;

    private bool hasStopped = false;
    private bool canPickup = false;

    void Start()
    {
        // 発射直後にプレイヤーへ当たって回収されるのを防ぐ
        Invoke(nameof(EnablePickup), 0.2f);
    }

    void EnablePickup()
    {
        canPickup = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("爆発弾が接触：" + other.name);

        // =========================
        // プレイヤーに触れた
        // =========================
        if (other.CompareTag("Player") && canPickup)
        {
            PlayerShoot playerShoot =
                other.GetComponent<PlayerShoot>();

            if (playerShoot != null)
            {
                // 通常弾として回収
                playerShoot.GetBullet();

                Destroy(gameObject);
            }

            return;
        }

        // =========================
        // すでに停止していたら何もしない
        // =========================
        if (hasStopped)
            return;


        // =========================
        // 通常敵に当たった
        // =========================
        Enemy enemy =
            other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            Explode();
            return;
        }


        // =========================
        // シューター敵に当たった
        // =========================
        EnemyShooter shooter =
            other.GetComponentInParent<EnemyShooter>();

        if (shooter != null)
        {
            Explode();
            return;
        }


        // =========================
        // 中ボスに当たった
        // =========================
        MiniBoss miniBoss =
            other.GetComponentInParent<MiniBoss>();

        if (miniBoss != null)
        {
            Explode();
            return;
        }

        // 大ボスに当たった
        FinalBossHealth finalBoss =
            other.GetComponentInParent<FinalBossHealth>();

        if (finalBoss != null)
        {
            Explode();
            return;
        }


        // =========================
        // 壁に当たった
        // =========================
        if (other.CompareTag("Wall"))
        {
            StopBullet();
        }
    }


    // =========================
    // 爆発
    // =========================
    void Explode()
    {
        Debug.Log("💥 爆発！");

        // 爆発エフェクトを表示
        if (explosionEffectPrefab != null)
        {
            Instantiate(
                explosionEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // 爆発範囲内の敵を取得
        Collider2D[] hitEnemies =
            Physics2D.OverlapCircleAll(
                transform.position,
                explosionRadius
            );

        foreach (Collider2D hit in hitEnemies)
        {
            // =========================
            // 通常敵
            // =========================
            Enemy enemy =
                hit.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                EnemyDrop drop =
                    enemy.GetComponent<EnemyDrop>();

                if (drop != null)
                {
                    drop.DropItem();
                }

                Destroy(enemy.gameObject);
            }


            // =========================
            // シューター敵
            // =========================
            EnemyShooter shooter =
                hit.GetComponentInParent<EnemyShooter>();

            if (shooter != null)
            {
                EnemyDrop drop =
                    shooter.GetComponent<EnemyDrop>();

                if (drop != null)
                {
                    drop.DropItem();
                }

                Destroy(shooter.gameObject);
            }


            // =========================
            // 中ボス
            // =========================
            MiniBoss miniBoss =
                hit.GetComponentInParent<MiniBoss>();

            if (miniBoss != null)
            {
                miniBoss.TakeDamage();
            }

            // 大ボスへのダメージ
            FinalBossHealth finalBoss =
                hit.GetComponentInParent<FinalBossHealth>();

            if (finalBoss != null)
            {
                finalBoss.TakeDamage(3);
            }
        }


        // 爆発弾は消さない
        // その場で停止
        StopBullet();
    }


    // =========================
    // 弾をその場で停止
    // =========================
    void StopBullet()
    {
        hasStopped = true;

        Rigidbody2D rb =
            GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log("爆発弾がその場で停止しました！");
    }


    // =========================
    // 爆発範囲をSceneビューで表示
    // =========================
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}