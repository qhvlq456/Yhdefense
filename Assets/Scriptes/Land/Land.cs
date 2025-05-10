using UnityEngine;

public class Land : MonoBehaviour 
{
    // Test¿ë
    [Header("Color")]
    [SerializeField]
    private Color enemyRoadColor = Color.red;

    [SerializeField]
    private Color mineRoadColor = Color.white;

    [SerializeField]
    private Color decoRoadColor = Color.green;

    [SerializeField]
    private MeshRenderer meshRenderer;

    private LandData data;

    public void Load(LandData _landData)
    {
        data = _landData;
        switch (data.landType)
        {
            case LandType.hero:
                meshRenderer.material.color = mineRoadColor;
                break;
            case LandType.enemy:
                meshRenderer.material.color = enemyRoadColor;
                break;
            case LandType.deco:
                meshRenderer.material.color = decoRoadColor;
                break;
        }
    }

    public void Retrieve()
    {
        ObjectPoolManager.Instance.RetrieveLand(transform);
    }
}
