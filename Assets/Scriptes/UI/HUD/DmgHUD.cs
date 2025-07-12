using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DmgHUD : BaseUI
{
    [SerializeField]
    private RectTransform mRect;

    [SerializeField]
    private TextMeshProUGUI dmgText;

    [SerializeField]
    private float fadeDuration = 0.25f;
    [SerializeField]
    private float fadeForce = 5f;

    [SerializeField]
    private Color color;
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    private IEnumerator CoStartDmgCoroutine = null;
    public void StartDmg(int _dmg, Transform _target)
    {
        dmgText.text = _dmg.ToString();
        dmgText.color = color;

        if (CoStartDmgCoroutine != null)
        {
            StopCoroutine(CoStartDmgCoroutine);
        }

        Vector3 screenPosition = Utility.WorldToScreenPoint(_target.position, offset);
        CoStartDmgCoroutine = CoStartDmg(_dmg, screenPosition);
        StartCoroutine(CoStartDmgCoroutine);
    }

    IEnumerator CoStartDmg(int _dmg, Vector3 _screenPosition)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration); // 0~1·Î º¸°£
            mRect.position = Vector3.Lerp(_screenPosition, _screenPosition + Vector3.up * fadeForce, t);
            yield return null;
        }

        UIManager.Instance.RecycleUI(UIPanelType.DmgText, this);
    }

    public override void DeactivateUI()
    {
        StopAllCoroutines();
        base.DeactivateUI();
    }
}
