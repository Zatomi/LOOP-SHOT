using UnityEngine;

public class FinalBossMove : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;

    void Start()
    {
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
    }

    void Update()
    {
        if (player == null) return;

        Vector2 direction =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);
    }
}