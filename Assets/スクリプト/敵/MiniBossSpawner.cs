
using UnityEngine;

public class MiniBossSpawner : MonoBehaviour
{
    public GameObject miniBossPrefab;
    public Transform player;
    public SpriteRenderer stageRenderer;

    public float spawnDistance = 8f;
    public float edgeMargin = 1f;
    public int maxAttempts = 20;

    void Start()
    {
        // 120秒後に最初の中ボス
        InvokeRepeating(nameof(SpawnMiniBoss), 120f, 120f);
    }

    void SpawnMiniBoss()
    {
        if (player == null || miniBossPrefab == null || stageRenderer == null)
            return;

        Bounds bounds = stageRenderer.bounds;

        for (int i = 0; i < maxAttempts; i++)
        {
            float angle = Random.Range(0f, 360f);

            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            Vector2 spawnPosition =
                (Vector2)player.position + direction * spawnDistance;

            // ステージの内側か確認
            if (spawnPosition.x < bounds.min.x + edgeMargin ||
                spawnPosition.x > bounds.max.x - edgeMargin ||
                spawnPosition.y < bounds.min.y + edgeMargin ||
                spawnPosition.y > bounds.max.y - edgeMargin)
            {
                continue;
            }

            // 壁やほかの敵などに重なっていないか確認
            Collider2D hit = Physics2D.OverlapCircle(
                spawnPosition,
                0.6f
            );

            if (hit == null)
            {
                Instantiate(
                    miniBossPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                Debug.Log("中ボスが出現！");
                return;
            }
        }

        Debug.Log("中ボスのスポーン場所が見つかりませんでした");
    }
}