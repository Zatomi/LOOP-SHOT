using UnityEngine;

public class SpecialBulletItem : MonoBehaviour
{
    public SpecialBulletType bulletType;

    [Header("æ“¾‰¹")]
    public AudioClip pickupSound;
    [Range(0f, 1f)]
    public float pickupVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerShoot playerShoot = other.GetComponent<PlayerShoot>();

        if (playerShoot == null)
            return;

        // “Áê’e‚ğæ“¾
        playerShoot.GetSpecialBullet(bulletType);

        // æ“¾‰¹‚ğÄ¶
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(
                pickupSound,
                transform.position,
                pickupVolume
            );
        }

        // ƒAƒCƒeƒ€‚ğíœ
        Destroy(gameObject);
    }
}