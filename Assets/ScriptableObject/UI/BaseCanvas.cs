using UnityEngine;

public class BaseCanvas : MonoBehaviour
{
    [SerializeField]
    protected Canvas canvas;
    public Canvas Canvas
    {
        get
        {
            if (canvas == null)
            {
                canvas = GetComponent<Canvas>();
                if (canvas == null)
                {
                    Debug.LogError("Canvas component not found on " + gameObject.name);
                }
            }
            // 후에 켄버스 셋팅도 설정
            return canvas;
        }
    }

    public virtual void Initialize() { }
    public virtual void UpdateCanvas() { }
}
