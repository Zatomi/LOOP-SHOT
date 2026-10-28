using UnityEngine;

public class HealItem : MonoBehaviour
{
    [Header("回復設定")]
    public AudioClip healSound;

    [Range(0f, 1f)]
    public float healVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth playerHealth =
            other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            return;

        // HPが最大なら拾わず、その場に残す
        if (playerHealth.IsFullHealth())
        {
            Debug.Log("HPが満タンなので回復アイテムを取得しません");
            return;
        }

        // HPを回復
        playerHealth.Heal();

        // 回復音を再生
        if (healSound != null)
        {
            AudioSource.PlayClipAtPoint(
                healSound,
                transform.position,
                healVolume
            );
        }

        // アイテムを削除
        Destroy(gameObject);
    }
}