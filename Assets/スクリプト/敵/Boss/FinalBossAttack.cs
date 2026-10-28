using UnityEngine;

public class FinalBossAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletCount = 16;
    public float attackInterval = 3f;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("大ボスのAnimatorが見つかりません");
        }

        InvokeRepeating(nameof(ShootAllDirections), 1f, attackInterval);
    }

    void ShootAllDirections()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("BossBulletのPrefabを設定してください");
            return;
        }

        // 攻撃モーションを再生
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            float radian = angle * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(
                Mathf.Cos(radian),
                Mathf.Sin(radian)
            );

            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            BossBullet bossBullet = bullet.GetComponent<BossBullet>();

            if (bossBullet != null)
            {
                bossBullet.SetDirection(direction);
            }
        }
    }
}