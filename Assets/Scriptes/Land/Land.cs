using UnityEngine;

public class Land : MonoBehaviour
{
    [SerializeField]
    protected MeshRenderer meshRenderer;

    private LandData data;

    public virtual void Create(LandData _data)
    {
        data = _data;
    }

    public virtual void Retrieve()
    {
        ObjectPoolManager.Instance.Retrieve(Utility.LandTypeToPoolingType(data.landType), data.index, transform);
    }
}
