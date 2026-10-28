using UnityEngine;

public class FinalBossMove : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // プレイヤーを取得
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグのオブジェクトが見つかりません");
        }

        // スプライトを取得
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRendererが見つかりません");
        }
    }

    void Update()
    {
        if (player == null) return;

        // プレイヤーの方向を計算
        Vector2 direction =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        // プレイヤーを追いかける
        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);

        // 左右の向きを変更
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }
}