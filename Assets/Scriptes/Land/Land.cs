using Unity.AI.Navigation;
using UnityEngine;

public class Land : MonoBehaviour
{
    [SerializeField]
    protected MeshRenderer meshRenderer;

    private LandData data;
    public LandData LandData => data;
    public virtual void Create(LandData _data)
    {
        data = _data;
    }
    public virtual void Retrieve()
    {
        ObjectPoolManager.Instance.Retrieve(Utility.LandTypeToPoolingType(data.landType), data.index, transform);
    }

    public virtual void SetColor(Color _color)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = _color;
        }
        else
        {
            Debug.LogError("MeshRenderer is not assigned in Land!");
        }
    }
}
