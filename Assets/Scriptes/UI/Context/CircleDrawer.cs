using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour
{
    [SerializeField] private int segments = 50;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;

        // 월드 좌표 기준 사용
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments;

        lineRenderer.widthMultiplier = 0.5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    public void DrawCircle(float _radius)
    {
        float angleStep = 360f / segments;
        // hero의 스케일을 고려하여 반지름을 조정
        float localRadius = _radius / transform.lossyScale.x;
        // 부모 위치를 중심으로 잡음
        Vector3 center = transform.position;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            float x = Mathf.Cos(angle) * localRadius;
            float z = Mathf.Sin(angle) * localRadius;

            Vector3 worldPos = new Vector3(x, 0.05f, z) + center;
            lineRenderer.SetPosition(i, worldPos);
        }
    }

}
