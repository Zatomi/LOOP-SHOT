using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHits = 5;
    public float invincibleTime = 1f;

    // 被ダメージ音
    public AudioClip damageSound;
    [Range(0f, 1f)]
    public float damageVolume = 1f;

    private AudioSource audioSource;

    // 左から Heart1 ～ Heart5 の順に登録する
    public Image[] hearts;

    private int currentHits;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHits = maxHits;

        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        UpdateHearts();
        Debug.Log("残り回数：" + currentHits);
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                // HPが減ると右端からハートを消す
                hearts[i].enabled = i < currentHits;
            }
        }
    }

    public void TakeHit()
    {
        if (isInvincible)
            return;

        currentHits--;
        PlayDamageSound();
        currentHits = Mathf.Max(0, currentHits);

        UpdateHearts();
        StartBlink();

        Debug.Log("敵に当たった！ 残り回数：" + currentHits);

        isInvincible = true;
        Invoke(nameof(EndInvincible), invincibleTime);

        if (currentHits <= 0)
        {
            GameOver();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        currentHits -= damage;
        PlayDamageSound();
        currentHits = Mathf.Max(0, currentHits);

        UpdateHearts();
        StartBlink();

        Debug.Log("ダメージ：" + damage + " 残り回数：" + currentHits);

        isInvincible = true;
        Invoke(nameof(EndInvincible), invincibleTime);

        if (currentHits <= 0)
        {
            GameOver();
        }
    }

    void EndInvincible()
    {
        isInvincible = false;
        Debug.Log("無敵終了");
    }

    public bool IsFullHealth()
    {
        return currentHits >= maxHits;
    }

    // HPアイテムで1回分回復
    public void Heal()
    {
        if (currentHits < maxHits)
        {
            currentHits++;
            UpdateHearts();

            Debug.Log("HP回復！ 残り回数：" + currentHits);
        }
        else
        {
            Debug.Log("HPは満タンです");
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");

        Time.timeScale = 1f;
        SceneManager.LoadScene("ゲームオーバー");
    }


    void StartBlink()
    {
        CancelInvoke(nameof(Blink));
        CancelInvoke(nameof(StopBlink));

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRendererが見つかりません");
            return;
        }

        spriteRenderer.enabled = true;
        InvokeRepeating(nameof(Blink), 0f, 0.1f);
        Invoke(nameof(StopBlink), 1f);
    }

    void Blink()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }

    void StopBlink()
    {
        CancelInvoke(nameof(Blink));

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }
    }

    void PlayDamageSound()
    {
        if (damageSound == null)
            return;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.PlayOneShot(damageSound, damageVolume);
    }
}
