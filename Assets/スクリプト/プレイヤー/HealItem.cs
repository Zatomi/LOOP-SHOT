using UnityEngine;

public class HealItem : MonoBehaviour
{
    [Header("‰ñ•œ‰¹")]
    public AudioClip healSound;

    [Range(0f, 1f)]
    public float healVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth playerHealth =
            other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // ‰ñ•œˆ—
            playerHealth.Heal();

            // ‰ñ•œ‰¹‚ğÄ¶
            if (healSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    healSound,
                    transform.position,
                    healVolume
                );
            }

            // ‰ñ•œƒAƒCƒeƒ€‚ğíœ
            Destroy(gameObject);
        }
    }
}