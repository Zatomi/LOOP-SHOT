using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public GameObject explosiveBulletPrefab;
    public Transform weapon;
    public GameObject lightningBulletPrefab;
    public GameObject piercingBulletPrefab;
    public GameObject splitBulletPrefab;

    public SpecialBulletUI specialBulletUI;


    [Header("発射音")]
    public AudioClip shootSound;
    [Range(0f, 1f)]
    public float shootVolume = 1f;

    private AudioSource audioSource;


    private bool hasBullet = true;

    // 拾った順番に特殊弾を保存
    private List<SpecialBulletType> specialBullets =
        new List<SpecialBulletType>();

    public int maxSpecialBullets = 5;

    // 選択中の弾：0は通常弾、1～5は特殊弾
    private int selectedBulletIndex = 0;

    void Update()
    {
        if (Mouse.current == null || Camera.main == null)
            return;

// マウスホイールで弾を選択
if (Mouse.current.scroll.ReadValue().y != 0)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            int scrollDirection = scroll > 0 ? -1 : 1;

            int nextIndex = selectedBulletIndex;

            // 空欄を飛ばして、選択できる弾を探す
            for (int i = 0; i < 6; i++)
            {
                nextIndex += scrollDirection;

                if (nextIndex < 0)
                    nextIndex = 5;
                else if (nextIndex > 5)
                    nextIndex = 0;

                // 通常弾は常に選択可能
                if (nextIndex == 0)
                    break;

                // 特殊弾が入っているスロットだけ選択可能
                int specialIndex = nextIndex - 1;

                if (specialIndex < specialBullets.Count)
                    break;
            }

            selectedBulletIndex = nextIndex;

            if (specialBulletUI != null)
                specialBulletUI.UpdateSelection(selectedBulletIndex);
        }

        Vector3 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldMousePosition =
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -Camera.main.transform.position.z
                )
            );

        worldMousePosition.z = 0;

        Vector2 direction =
            ((Vector2)worldMousePosition -
             (Vector2)transform.position).normalized;

        if (weapon != null)
        {
            SpriteRenderer weaponSprite =
                weapon.GetComponent<SpriteRenderer>();

            float angle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (direction.x < 0)
            {
                weapon.localPosition = new Vector3(-0.3f, -0.15f, 0);

                if (weaponSprite != null)
                    weaponSprite.flipX = true;

                weapon.rotation =
                    Quaternion.Euler(0, 0, angle + 180f);
            }
            else
            {
                weapon.localPosition = new Vector3(0.3f, -0.15f, 0);

                if (weaponSprite != null)
                    weaponSprite.flipX = false;

                weapon.rotation =
                    Quaternion.Euler(0, 0, angle);
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && hasBullet)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        audioSource = GetComponent<AudioSource>();

        Vector3 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldMousePosition =
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -Camera.main.transform.position.z
                )
            );

        worldMousePosition.z = 0;

        Vector2 direction =
            ((Vector2)worldMousePosition -
             (Vector2)transform.position).normalized;

        Vector3 spawnPosition =
            transform.position + (Vector3)(direction * 0.8f);

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject selectedBulletPrefab;

        // 0なら通常弾、1～5なら特殊弾
        if (selectedBulletIndex == 0)
        {
            selectedBulletPrefab = bulletPrefab;
            Debug.Log("通常弾を選択中！");
        }
        else
        {
            int specialIndex = selectedBulletIndex - 1;

            // 空欄が選択されていた場合は発射しない
            if (specialIndex < 0 || specialIndex >= specialBullets.Count)
                return;

            SpecialBulletType type = specialBullets[specialIndex];
            selectedBulletPrefab = GetBulletPrefab(type);

            if (selectedBulletPrefab == null)
            {
                Debug.LogError(type + " の弾Prefabが設定されていません！");
                return;
            }

            // 発射した弾だけを所持リストから削除
            specialBullets.RemoveAt(specialIndex);

            Debug.Log(type + "を発射！");
            UpdateSpecialBulletUI();

            // 発射後、選択位置を調整
            if (selectedBulletIndex > specialBullets.Count)
                selectedBulletIndex = specialBullets.Count;

            if (specialBulletUI != null)
                specialBulletUI.UpdateSelection(selectedBulletIndex);
        }

        if (selectedBulletPrefab == null)
        {
            Debug.LogError("発射する弾のPrefabが設定されていません！");
            return;
        }

        GameObject bullet = Instantiate(
            selectedBulletPrefab,
            spawnPosition,
            Quaternion.Euler(0, 0, angle - 90f)
        );

        // 弾の発射音
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(
                shootSound,
                transform.position,
                shootVolume
            );
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        hasBullet = false;
    }

    GameObject GetBulletPrefab(SpecialBulletType type)
    {
        switch (type)
        {
            case SpecialBulletType.Explosive:
                return explosiveBulletPrefab;

            case SpecialBulletType.Piercing:
                return piercingBulletPrefab;

            case SpecialBulletType.Split:
                return splitBulletPrefab;

            case SpecialBulletType.Lightning:
                return lightningBulletPrefab;

            default:
                return null;
        }
    }

    public void GetBullet()
    {
        hasBullet = true;
        Debug.Log("弾を回収しました！");
    }

    public void GetSpecialBullet(SpecialBulletType type)
    {
        if (specialBullets.Count >= maxSpecialBullets)
        {
            Debug.Log("特殊弾は最大5個です！");
            return;
        }

        specialBullets.Add(type);

        Debug.Log(type + "を取得しました！");
        UpdateSpecialBulletUI();
    }

    void UpdateSpecialBulletUI()
    {
        if (specialBulletUI != null)
        {
            specialBulletUI.UpdateSlots(specialBullets);
        }
    }
}