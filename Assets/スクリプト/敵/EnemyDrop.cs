using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public GameObject healItemPrefab;

    // 特殊弾アイテムを複数登録
    public GameObject[] specialBulletItemPrefabs;

    public float healChance = 40f;
    public float specialBulletChance = 20f;

    public void DropItem()
    {
        float randomValue = Random.Range(0f, 100f);

        // =========================
        // HP回復
        // =========================
        if (randomValue < healChance)
        {
            if (healItemPrefab != null)
            {
                Instantiate(
                    healItemPrefab,
                    transform.position,
                    Quaternion.identity
                );
            }

            Debug.Log("HP回復アイテムがドロップ！");
        }

        // =========================
        // 特殊弾
        // =========================
        else if (randomValue < healChance + specialBulletChance)
        {
            DropRandomSpecialBullet();
        }

        // =========================
        // 何もなし
        // =========================
        else
        {
            Debug.Log("何もドロップしなかった");
        }
    }


    // =========================
    // ランダムな特殊弾を落とす
    // =========================
    void DropRandomSpecialBullet()
    {
        if (specialBulletItemPrefabs == null ||
            specialBulletItemPrefabs.Length == 0)
        {
            Debug.LogWarning("特殊弾アイテムが登録されていません！");
            return;
        }

        // 特殊弾をランダムに選ぶ
        int randomIndex =
            Random.Range(0, specialBulletItemPrefabs.Length);

        GameObject selectedBullet =
            specialBulletItemPrefabs[randomIndex];

        if (selectedBullet != null)
        {
            Instantiate(
                selectedBullet,
                transform.position,
                Quaternion.identity
            );

            Debug.Log(
                "特殊弾がドロップ！：" +
                selectedBullet.name
            );
        }
    }


    // =========================
    // 中ボス用
    // =========================
    public void DropBossItems()
    {
        // 回復アイテム1個
        if (healItemPrefab != null)
        {
            Instantiate(
                healItemPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // 特殊弾2個
        for (int i = 0; i < 2; i++)
        {
            DropRandomSpecialBullet();
        }

        Debug.Log(
            "中ボス撃破！ 回復アイテム×1、特殊弾×2！"
        );
    }
}