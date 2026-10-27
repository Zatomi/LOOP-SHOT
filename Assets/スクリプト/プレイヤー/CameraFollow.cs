
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public SpriteRenderer stageRenderer;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (target == null || stageRenderer == null)
        {
            Debug.LogError("CameraFollow：TargetとStage Rendererを設定してください。");
        }
    }

    void LateUpdate()
    {
        if (target == null || stageRenderer == null || cam == null)
            return;

        Bounds bounds = stageRenderer.bounds;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float minX = bounds.min.x + halfWidth;
        float maxX = bounds.max.x - halfWidth;
        float minY = bounds.min.y + halfHeight;
        float maxY = bounds.max.y - halfHeight;

        float x = target.position.x;
        float y = target.position.y;

        // ステージが画面より小さい場合は、その中心に固定
        if (minX > maxX)
            x = bounds.center.x;
        else
            x = Mathf.Clamp(x, minX, maxX);

        if (minY > maxY)
            y = bounds.center.y;
        else
            y = Mathf.Clamp(y, minY, maxY);

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
