using UnityEngine;

public class LandOutline : MonoBehaviour
{
    [SerializeField]
    private Transform parent;
    // 각 면을 그릴 LineRenderer 컴포넌트들
    public LineRenderer frontRenderer;
    public LineRenderer backRenderer;
    public LineRenderer topRenderer;
    public LineRenderer bottomRenderer;
    // Note: Left/Right Face는 Front/Back/Top/Bottom으로 커버됨 (모서리 중복)

    [Header("큐브 설정")]
    public float sizeX = 1f;              // 큐브의 X축 길이
    public float sizeY = 1f;              // 큐브의 Y축 길이
    public float sizeZ = 1f;              // 큐브의 Z축 길이

    [Header("Line Renderer 공통 설정")]
    public float lineWidth = 0.05f;      // 선의 두께
    public Color lineColor = Color.blue; // 선의 색상

    void Start()
    {
        SetupLineRenderers();
        DrawCubeFaces();
    }

    private void SetupLineRenderers()
    {
        // 각 Line Renderer가 할당되어 있는지 확인하고 없으면 생성
        frontRenderer = GetOrCreateLineRenderer("FrontLineRenderer", frontRenderer);
        backRenderer = GetOrCreateLineRenderer("BackLineRenderer", backRenderer);
        topRenderer = GetOrCreateLineRenderer("TopLineRenderer", topRenderer);
        bottomRenderer = GetOrCreateLineRenderer("BottomLineRenderer", bottomRenderer);
    }

    private LineRenderer GetOrCreateLineRenderer(string goName, LineRenderer existingRenderer)
    {
        if (existingRenderer == null)
        {
            GameObject go = new GameObject(goName);
            go.transform.SetParent(parent); // 메인 GameObject의 자식으로 설정
            existingRenderer = go.AddComponent<LineRenderer>();

            // 공통 설정 적용
            existingRenderer.material = new Material(Shader.Find("Sprites/Default"));
            existingRenderer.startWidth = lineWidth;
            existingRenderer.endWidth = lineWidth;
            existingRenderer.startColor = lineColor;
            existingRenderer.endColor = lineColor;
            existingRenderer.loop = true; // 중요: 사각형 면을 그리기 위해 loop true 설정
            existingRenderer.useWorldSpace = false; // 로컬 좌표 사용
        }
        return existingRenderer;
    }


    void DrawCubeFaces()
    {
        float halfX = sizeX / 2f;
        float halfY = sizeY / 2f;
        float halfZ = sizeZ / 2f;

        // 큐브의 8개 꼭짓점 계산
        // F = Front (Z+), B = Back (Z-)
        // L = Left (X-), R = Right (X+)
        // D = Down (Y-), U = Up (Y+)
        Vector3 center = transform.localPosition;
        // 하단 4개 꼭짓점
        Vector3 p_BLD = center + new Vector3(-halfX, -halfY, -halfZ); // Back-Left-Down
        Vector3 p_BRD = center + new Vector3(halfX, -halfY, -halfZ);  // Back-Right-Down
        Vector3 p_FLD = center + new Vector3(-halfX, -halfY, halfZ);  // Front-Left-Down
        Vector3 p_FRD = center + new Vector3(halfX, -halfY, halfZ);   // Front-Right-Down

        // 상단 4개 꼭짓점
        Vector3 p_BLU = center + new Vector3(-halfX, halfY, -halfZ);  // Back-Left-Up
        Vector3 p_BRU = center + new Vector3(halfX, halfY, -halfZ);   // Back-Right-Up
        Vector3 p_FLU = center + new Vector3(-halfX, halfY, halfZ);   // Front-Left-Up
        Vector3 p_FRU = center + new Vector3(halfX, halfY, halfZ);    // Front-Right-Up

        // 각 면의 꼭짓점을 LineRenderer에 설정 (loop=true 이므로 4점만 필요)

        // 1. Front Face (앞면: Z+ 방향)
        frontRenderer.positionCount = 4;
        frontRenderer.SetPosition(0, p_FLD); // Front-Left-Down
        frontRenderer.SetPosition(1, p_FRD); // Front-Right-Down
        frontRenderer.SetPosition(2, p_FRU); // Front-Right-Up
        frontRenderer.SetPosition(3, p_FLU); // Front-Left-Up

        // 2. Back Face (뒷면: Z- 방향)
        backRenderer.positionCount = 4;
        backRenderer.SetPosition(0, p_BRD); // Back-Right-Down
        backRenderer.SetPosition(1, p_BLD); // Back-Left-Down
        backRenderer.SetPosition(2, p_BLU); // Back-Left-Up
        backRenderer.SetPosition(3, p_BRU); // Back-Right-Up

        // 3. Top Face (상단면: Y+ 방향)
        topRenderer.positionCount = 4;
        topRenderer.SetPosition(0, p_FLU); // Front-Left-Up
        topRenderer.SetPosition(1, p_FRU); // Front-Right-Up
        topRenderer.SetPosition(2, p_BRU); // Back-Right-Up
        topRenderer.SetPosition(3, p_BLU); // Back-Left-Up

        // 4. Bottom Face (하단면: Y- 방향)
        bottomRenderer.positionCount = 4;
        bottomRenderer.SetPosition(0, p_BLD); // Back-Left-Down
        bottomRenderer.SetPosition(1, p_BRD); // Back-Right-Down
        bottomRenderer.SetPosition(2, p_FRD); // Front-Right-Down
        bottomRenderer.SetPosition(3, p_FLD); // Front-Left-Down
    }
}