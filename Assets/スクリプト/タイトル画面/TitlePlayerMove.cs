
using UnityEngine;

public class TitlePlayerMove : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2f;
    public float changeDirectionTime = 2f;

    [Header("カメラ内の移動範囲")]
    public float edgeMargin = 0.5f;

    [Header("武器の位置")]
    public float weaponSideOffset = 0.328f;

    private Vector2 moveDirection;
    private float timer;

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    private Transform weaponTransform;
    private Vector3 weaponOriginalPosition;
    private Vector3 weaponOriginalScale;

    private bool facingLeft = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        weaponTransform = transform.Find("Weapon");

        if (weaponTransform != null)
        {
            weaponOriginalPosition = weaponTransform.localPosition;
            weaponOriginalScale = weaponTransform.localScale;
        }
        else
        {
            Debug.LogWarning("Playerの子オブジェクトにWeaponが見つかりません。");
        }

        ChangeDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ChangeDirection();
        }

        Vector3 nextPosition = transform.position
            + (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        if (mainCamera != null)
        {
            float distance = -mainCamera.transform.position.z;

            Vector3 min = mainCamera.ViewportToWorldPoint(
                new Vector3(0f, 0f, distance));

            Vector3 max = mainCamera.ViewportToWorldPoint(
                new Vector3(1f, 1f, distance));

            min.x += edgeMargin;
            min.y += edgeMargin;
            max.x -= edgeMargin;
            max.y -= edgeMargin;

            nextPosition.x = Mathf.Clamp(nextPosition.x, min.x, max.x);
            nextPosition.y = Mathf.Clamp(nextPosition.y, min.y, max.y);
        }

        transform.position = nextPosition;

        if (moveDirection.x > 0.1f)
        {
            SetFacing(false);
        }
        else if (moveDirection.x < -0.1f)
        {
            SetFacing(true);
        }
    }

    void ChangeDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        timer = Random.Range(1f, Mathf.Max(1.01f, changeDirectionTime));
    }

    void SetFacing(bool faceLeft)
    {
        if (facingLeft == faceLeft)
            return;

        facingLeft = faceLeft;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = facingLeft;
        }

        if (weaponTransform != null)
        {
            Vector3 pos = weaponOriginalPosition;
            pos.x = Mathf.Abs(weaponSideOffset)
                * (facingLeft ? -1f : 1f);

            weaponTransform.localPosition = pos;

            Vector3 scale = weaponOriginalScale;
            scale.x = Mathf.Abs(weaponOriginalScale.x)
                * (facingLeft ? -1f : 1f);

            weaponTransform.localScale = scale;
        }
    }
}
