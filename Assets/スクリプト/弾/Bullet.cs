using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool canPickup = false;
    private bool hasHitEnemy = false;

    public AudioClip hitSound;

    [Range(0f, 1f)]
    public float hitVolume = 1f;

    void Start()
    {
        Invoke(nameof(EnablePickup), 0.2f);
    }

    void EnablePickup()
    {
        canPickup = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーに触れた
        if (other.CompareTag("Player") && canPickup)
        {
            PlayerShoot playerShoot =
                other.GetComponent<PlayerShoot>();

            if (playerShoot != null)
            {
                playerShoot.GetBullet();
                Destroy(gameObject);
            }

            return;
        }

        // 大ボスに当たった
        FinalBossHealth finalBoss = other.GetComponent<FinalBossHealth>();

        if (finalBoss != null)
        {
            if (hasHitEnemy)
                return;

            PlayHitSound();

            hasHitEnemy = true;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            finalBoss.TakeDamage(1);

            return;
        }

        MiniBoss miniBoss = other.GetComponent<MiniBoss>();

        if (miniBoss != null)
        {
            if (hasHitEnemy)
                return;

            hasHitEnemy = true;

            PlayHitSound();

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            miniBoss.TakeDamage();

            return;
        }

        // 敵に当たった
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (hasHitEnemy)
                return;

            hasHitEnemy = true;

            PlayHitSound();

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            // ドロップ
            EnemyDrop drop = enemy.GetComponent<EnemyDrop>();

            if (drop != null)
            {
                drop.DropItem();
            }

            Destroy(enemy.gameObject);
            return;
        }

        // 壁に当たった
        if (other.CompareTag("Wall"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void HitEnemy()
    {
        hasHitEnemy = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public bool HasHitEnemy()
    {
        return hasHitEnemy;
    }

    void PlayHitSound()
    {
        Debug.Log("ヒット音の処理を実行");

        if (hitSound == null)
        {
            Debug.LogError("Hit Soundが設定されていません！");
            return;
        }

        AudioSource.PlayClipAtPoint(
            hitSound,
            Camera.main != null ? Camera.main.transform.position : transform.position,
            hitVolume
        );

        Debug.Log("ヒット音を再生：" + hitSound.name);
    }

}