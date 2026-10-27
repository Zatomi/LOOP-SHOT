
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public SpriteRenderer stageRenderer;

    public float spawnDistance = 8f;
    public float spawnInterval = 3f;
    public int maxAttempts = 20;

    // ステージの端から敵を離す距離
    public float edgeMargin = 1f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (player == null || enemyPrefab == null || stageRenderer == null)
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

            // 敵や壁などに重なっていないか確認
            Collider2D hit = Physics2D.OverlapCircle(
                spawnPosition,
                0.6f
            );

            if (hit == null)
            {
                Instantiate(
                    enemyPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                return;
            }
        }

        Debug.Log("スポーンできる場所が見つかりませんでした");
    }
}