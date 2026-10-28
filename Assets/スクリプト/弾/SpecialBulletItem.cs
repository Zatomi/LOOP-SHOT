using UnityEngine;

public class SpecialBulletItem : MonoBehaviour
{
    public SpecialBulletType bulletType;

    [Header("取得設定")]
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

        // 特殊弾が満タンなら拾わず、その場に残す
        if (playerShoot.IsSpecialBulletFull())
        {
            Debug.Log("特殊弾が満タンなので取得しません");
            return;
        }

        // 特殊弾を取得
        playerShoot.GetSpecialBullet(bulletType);

        // 取得音を再生
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(
                pickupSound,
                transform.position,
                pickupVolume
            );
        }

        // アイテムを削除
        Destroy(gameObject);
    }
}