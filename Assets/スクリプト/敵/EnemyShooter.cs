using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    [Header("’e‚Ìİ’è")]
    public GameObject bulletPrefab;
    public float shootInterval = 2f;
    public float bulletSpeed = 5f;

    [Header("‰æ‘œ‚Ìİ’è")]
    public SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite attackSprite;
    public float attackImageDuration = 0.2f;

    private Transform player;
    private Coroutine attackCoroutine;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Å‰‚Í‘Ò‹@‰æ‘œ
        if (spriteRenderer != null && idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }

        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }

    void Shoot()
    {
        if (player == null || bulletPrefab == null)
            return;

        // Player‚Ì•ûŒü
        Vector2 direction =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        // “G‚Ì­‚µ‘O‚©‚ç’e‚ğo‚·
        Vector2 spawnPosition =
            (Vector2)transform.position + direction * 0.6f;

        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        // UŒ‚‰æ‘œ‚ÉØ‚è‘Ö‚¦‚é
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        attackCoroutine = StartCoroutine(ShowAttackImage());
    }

    IEnumerator ShowAttackImage()
    {
        if (spriteRenderer != null && attackSprite != null)
        {
            spriteRenderer.sprite = attackSprite;
        }

        yield return new WaitForSeconds(attackImageDuration);

        if (spriteRenderer != null && idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }

        attackCoroutine = null;
    }

    // ƒvƒŒƒCƒ„[‚Ì’e‚ª“–‚½‚Á‚½‚ç“|‚·
    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.HitEnemy();

            EnemyDrop drop = GetComponent<EnemyDrop>();

            if (drop != null)
            {
                drop.DropItem();
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (player == null)
            return;

        Vector3 scale = initialScale;

        if (player.position.x < transform.position.x)
        {
            // Player‚ª¶‚É‚¢‚éê‡
            scale.x = Mathf.Abs(initialScale.x);
        }
        else
        {
            // Player‚ª‰E‚É‚¢‚éê‡
            scale.x = -Mathf.Abs(initialScale.x);
        }

        transform.localScale = scale;
    }
}