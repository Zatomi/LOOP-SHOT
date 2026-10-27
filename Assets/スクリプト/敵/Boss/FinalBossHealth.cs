
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossHealth : MonoBehaviour
{
    public int maxHP = 15;
    private int currentHP;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        Debug.Log("大ボス出現！ HP：" + currentHP);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        Debug.Log("大ボスのHP：" + currentHP + " / " + maxHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("大ボス撃破！ゲームクリア！");

        Time.timeScale = 1f;
        SceneManager.LoadScene("ゲームクリア");
    }
}
