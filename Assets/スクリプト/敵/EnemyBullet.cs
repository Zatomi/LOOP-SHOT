using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player‚É“–‚½‚Á‚½
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeHit();
            }

            Destroy(gameObject);
        }

        // •Ç‚É“–‚½‚Á‚½
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}