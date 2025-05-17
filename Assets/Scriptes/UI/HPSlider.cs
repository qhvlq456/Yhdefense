using UnityEngine;
using UnityEngine.UI;

public class HPSlider : BaseUI
{
    [SerializeField]
    private RectTransform mRect;
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    private Transform target;
    public void SetTarget(Transform _target)
    {
        target = _target;
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
