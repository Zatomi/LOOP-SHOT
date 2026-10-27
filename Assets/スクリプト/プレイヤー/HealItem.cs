using UnityEngine;

public class HealItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal();

                Destroy(gameObject);
            }
        }
    }
}