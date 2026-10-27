
using System.Collections.Generic;
using UnityEngine;

public class LightningBullet : MonoBehaviour
{
    [Header("雷撃設定")]
    public float lightningRadius = 2f;
    public int damage = 2;

    [Header("雷の音")]
    public AudioClip lightningSound;
    [Range(0f, 1f)]
    public float lightningVolume = 1f;

    [Header("雷撃エフェクト")]
    public GameObject lightningEffectPrefab;

    private bool hasStopped = false;
    private bool canPickup = false;

    private HashSet<int> hitEnemies = new HashSet<int>();

    void Start()
    {
        // 発射直後の誤回収を防ぐ
        Invoke(nameof(EnablePickup), 0.2f);
    }

    void EnablePickup()
    {
        canPickup = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーが弾を回収
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

        if (hasStopped)
            return;

        // 壁に当たったら停止
        if (other.CompareTag("Wall"))
        {
            StopBullet();
            return;
        }

        // 大ボスに当たったら周囲にも雷撃
        FinalBossHealth finalBoss =
            other.GetComponentInParent<FinalBossHealth>();

        if (finalBoss != null)
        {
            StrikeLightning();
            return;
        }

        // 敵に当たったら周囲にも雷撃
        if (IsEnemy(other))
        {
            StrikeLightning();
        }
    }

    bool IsEnemy(Collider2D target)
    {
        return target.GetComponentInParent<Enemy>() != null
            || target.GetComponentInParent<EnemyShooter>() != null
            || target.GetComponentInParent<MiniBoss>() != null;
    }

    void StrikeLightning()
    {
        hasStopped = true;

        // 雷の音を再生
        if (lightningSound != null)
        {
            AudioSource.PlayClipAtPoint(
                lightningSound,
                transform.position,
                lightningVolume
            );
        }

        Debug.Log("雷撃！ 周囲の敵にもダメージ！");

        Collider2D[] targets =
            Physics2D.OverlapCircleAll(
                transform.position,
                lightningRadius
            );

        foreach (Collider2D target in targets)
        {
            ApplyDamage(target);
        }

        StopBullet();
    }

    void ApplyDamage(Collider2D target)
    {
        // 大ボス
        FinalBossHealth finalBoss =
            target.GetComponentInParent<FinalBossHealth>();

        if (finalBoss != null)
        {
            if (!RegisterHit(finalBoss.gameObject))
                return;

            ShowLightning(finalBoss.transform.position);
            finalBoss.TakeDamage(damage);
            return;
        }

        // 中ボス
        MiniBoss miniBoss =
            target.GetComponentInParent<MiniBoss>();

        if (miniBoss != null)
        {
            if (!RegisterHit(miniBoss.gameObject))
                return;

            ShowLightning(miniBoss.transform.position);
            miniBoss.TakeDamage(damage);
            return;
        }

        // 通常敵
        Enemy enemy =
            target.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            if (!RegisterHit(enemy.gameObject))
                return;

            ShowLightning(enemy.transform.position);

            EnemyDrop drop =
                enemy.GetComponent<EnemyDrop>();

            if (drop != null)
                drop.DropItem();

            Destroy(enemy.gameObject);
            return;
        }

        // シューター敵
        EnemyShooter shooter =
            target.GetComponentInParent<EnemyShooter>();

        if (shooter != null)
        {
            if (!RegisterHit(shooter.gameObject))
                return;

            ShowLightning(shooter.transform.position);

            EnemyDrop drop =
                shooter.GetComponent<EnemyDrop>();

            if (drop != null)
                drop.DropItem();

            Destroy(shooter.gameObject);
        }
    }

    bool RegisterHit(GameObject enemy)
    {
        return hitEnemies.Add(enemy.GetInstanceID());
    }

    void ShowLightning(Vector3 position)
    {
        if (lightningEffectPrefab != null)
        {
            Instantiate(
                lightningEffectPrefab,
                position,
                Quaternion.identity
            );
        }
    }

    void StopBullet()
    {
        hasStopped = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, lightningRadius);
    }
}
