using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    [SerializeField]
    private RectTransform mRect;
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    private Transform target;
    
    public void Attach()
    {

    }
    public void FollowTarget()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position);
        screenPosition = screenPosition + offset;

        mRect.position = screenPosition;
    }

    public void OnChangedValue(float _value)
    {
        slider.value = Mathf.Clamp01(_value);
    }
}
