using UnityEngine;

public class BossExplosionEffect : MonoBehaviour
{
    public float radius = 1.5f;
    public float duration = 0.3f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = 40;
        lineRenderer.startWidth = 0.06f;
        lineRenderer.endWidth = 0.06f;

        for (int i = 0; i < 40; i++)
        {
            float angle = i * Mathf.PI * 2f / 40;
            lineRenderer.SetPosition(i, new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            ));
        }

        Destroy(gameObject, duration);
    }
}
