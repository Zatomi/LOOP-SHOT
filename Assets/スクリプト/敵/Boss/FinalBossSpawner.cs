
using UnityEngine;

public class FinalBossSpawner : MonoBehaviour
{
    public GameObject finalBossPrefab;
    public float spawnTime = 600f; // 10分

    private float timer = 0f;
    private bool hasSpawned = false;

    void Update()
    {
        if (hasSpawned)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (finalBossPrefab == null)
        {
            Debug.LogError("大ボスのPrefabが設定されていません！");
            return;
        }

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        Vector3 spawnPosition = Vector3.zero;

        if (player != null)
        {
            // プレイヤーから少し離れた位置に出現
            spawnPosition = player.transform.position
                + new Vector3(8f, 0f, 0f);
        }

        Instantiate(
            finalBossPrefab,
            spawnPosition,
            Quaternion.identity
        );

        BGMManager bgm = FindFirstObjectByType<BGMManager>();

        if (bgm != null)
        {
            bgm.PlayBossBGM();
        }

        hasSpawned = true;
        Debug.Log("10分経過！大ボス出現！");
    }
}
