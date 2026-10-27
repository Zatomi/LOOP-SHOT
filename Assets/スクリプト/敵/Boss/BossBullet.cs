using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    public float explosionDelay = 0.5f;
    public float explosionRadius = 1.5f;
    public GameObject explosionEffectPrefab;
    public GameObject holyEffectPrefab;

    private Vector2 direction;
    private bool hasHit = false;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        if (!hasHit)
        {
            transform.position +=
                (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health == null)
                health = other.GetComponentInParent<PlayerHealth>();

            if (health != null)
                health.TakeHit();

            HitTarget();
        }
        else if (!other.CompareTag("Boss"))
        {
            HitTarget();
        }
    }


    void HitTarget()
    {
        if (hasHit)
            return;

        hasHit = true;

        // 弾が止まった場所に爆発範囲を表示
        if (explosionEffectPrefab != null)
        {
            Instantiate(
                explosionEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Invoke(nameof(Explode), explosionDelay);
    }



    void Explode()
    {
        // Holyエフェクトを生成し、1秒後に削除
        if (holyEffectPrefab != null)
        {
            GameObject holyEffect = Instantiate(
                holyEffectPrefab,
                transform.position,
                Quaternion.identity
            );

            Destroy(holyEffect, 0.5f);
        }

        // 爆発範囲を表示
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                explosionEffectPrefab,
                transform.position,
                Quaternion.identity
            );

            Destroy(effect, 0.3f);
        }

        // 爆発範囲内のプレイヤーに2ダメージ
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Player"))
                continue;

            PlayerHealth health = hit.GetComponent<PlayerHealth>();

            if (health == null)
                health = hit.GetComponentInParent<PlayerHealth>();

            if (health != null)
                health.TakeDamage(2);
        }

        Destroy(gameObject);
    }

}
