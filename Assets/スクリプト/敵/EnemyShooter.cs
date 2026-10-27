using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 2f;
    public float bulletSpeed = 5f;

    private Transform player;

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }

    void Shoot()
    {
        if (player == null || bulletPrefab == null)
            return;

        // Player‚Ì•ûŒü
        Vector2 direction =
            ((Vector2)player.position - (Vector2)transform.position).normalized;

        // “G‚Ì­‚µ‘O‚©‚ç’e‚ğo‚·
        Vector2 spawnPosition =
            (Vector2)transform.position + direction * 0.6f;

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                spawnPosition,
                Quaternion.identity
            );

        Rigidbody2D rb =
            bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }

    // ƒvƒŒƒCƒ„[‚Ì’e‚ª“–‚½‚Á‚½‚ç“|‚·
    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.HitEnemy();

            // ƒhƒƒbƒv
            EnemyDrop drop = GetComponent<EnemyDrop>();

            if (drop != null)
            {
                drop.DropItem();
            }

            // “G‚ğ“|‚·
            Destroy(gameObject);
        }
    }
}