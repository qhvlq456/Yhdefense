using UnityEngine;

public class LandOutline : MonoBehaviour
{
    [SerializeField]
    private Transform parent;
    // �� ���� �׸� LineRenderer ������Ʈ��
    public LineRenderer frontRenderer;
    public LineRenderer backRenderer;
    public LineRenderer topRenderer;
    public LineRenderer bottomRenderer;
    // Note: Left/Right Face�� Front/Back/Top/Bottom���� Ŀ���� (�𼭸� �ߺ�)

    [Header("ť�� ����")]
    public float sizeX = 1f;              // ť���� X�� ����
    public float sizeY = 1f;              // ť���� Y�� ����
    public float sizeZ = 1f;              // ť���� Z�� ����

    [Header("Line Renderer ���� ����")]
    public float lineWidth = 0.05f;      // ���� �β�
    public Color lineColor = Color.blue; // ���� ����

    void Start()
    {
        SetupLineRenderers();
        DrawCubeFaces();
    }

    private void SetupLineRenderers()
    {
        // �� Line Renderer�� �Ҵ�Ǿ� �ִ��� Ȯ���ϰ� ������ ����
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
            go.transform.SetParent(parent); // ���� GameObject�� �ڽ����� ����
            existingRenderer = go.AddComponent<LineRenderer>();

            // ���� ���� ����
            existingRenderer.material = new Material(Shader.Find("Sprites/Default"));
            existingRenderer.startWidth = lineWidth;
            existingRenderer.endWidth = lineWidth;
            existingRenderer.startColor = lineColor;
            existingRenderer.endColor = lineColor;
            existingRenderer.loop = true; // �߿�: �簢�� ���� �׸��� ���� loop true ����
            existingRenderer.useWorldSpace = false; // ���� ��ǥ ���
        }
        return existingRenderer;
    }


    void DrawCubeFaces()
    {
        float halfX = sizeX / 2f;
        float halfY = sizeY / 2f;
        float halfZ = sizeZ / 2f;

        // ť���� 8�� ������ ���
        // F = Front (Z+), B = Back (Z-)
        // L = Left (X-), R = Right (X+)
        // D = Down (Y-), U = Up (Y+)
        Vector3 center = transform.localPosition;
        // �ϴ� 4�� ������
        Vector3 p_BLD = center + new Vector3(-halfX, -halfY, -halfZ); // Back-Left-Down
        Vector3 p_BRD = center + new Vector3(halfX, -halfY, -halfZ);  // Back-Right-Down
        Vector3 p_FLD = center + new Vector3(-halfX, -halfY, halfZ);  // Front-Left-Down
        Vector3 p_FRD = center + new Vector3(halfX, -halfY, halfZ);   // Front-Right-Down

        // ��� 4�� ������
        Vector3 p_BLU = center + new Vector3(-halfX, halfY, -halfZ);  // Back-Left-Up
        Vector3 p_BRU = center + new Vector3(halfX, halfY, -halfZ);   // Back-Right-Up
        Vector3 p_FLU = center + new Vector3(-halfX, halfY, halfZ);   // Front-Left-Up
        Vector3 p_FRU = center + new Vector3(halfX, halfY, halfZ);    // Front-Right-Up

        // �� ���� �������� LineRenderer�� ���� (loop=true �̹Ƿ� 4���� �ʿ�)

        // 1. Front Face (�ո�: Z+ ����)
        frontRenderer.positionCount = 4;
        frontRenderer.SetPosition(0, p_FLD); // Front-Left-Down
        frontRenderer.SetPosition(1, p_FRD); // Front-Right-Down
        frontRenderer.SetPosition(2, p_FRU); // Front-Right-Up
        frontRenderer.SetPosition(3, p_FLU); // Front-Left-Up

        // 2. Back Face (�޸�: Z- ����)
        backRenderer.positionCount = 4;
        backRenderer.SetPosition(0, p_BRD); // Back-Right-Down
        backRenderer.SetPosition(1, p_BLD); // Back-Left-Down
        backRenderer.SetPosition(2, p_BLU); // Back-Left-Up
        backRenderer.SetPosition(3, p_BRU); // Back-Right-Up

        // 3. Top Face (��ܸ�: Y+ ����)
        topRenderer.positionCount = 4;
        topRenderer.SetPosition(0, p_FLU); // Front-Left-Up
        topRenderer.SetPosition(1, p_FRU); // Front-Right-Up
        topRenderer.SetPosition(2, p_BRU); // Back-Right-Up
        topRenderer.SetPosition(3, p_BLU); // Back-Left-Up

        // 4. Bottom Face (�ϴܸ�: Y- ����)
        bottomRenderer.positionCount = 4;
        bottomRenderer.SetPosition(0, p_BLD); // Back-Left-Down
        bottomRenderer.SetPosition(1, p_BRD); // Back-Right-Down
        bottomRenderer.SetPosition(2, p_FRD); // Front-Right-Down
        bottomRenderer.SetPosition(3, p_FLD); // Front-Left-Down
    }
}