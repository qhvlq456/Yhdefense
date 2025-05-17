using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private HPSlider hpSlider;
    public float currentHealth { get; private set; }
    public float maxHealth { get; private set; }
    public void ResetHealth(float _max)
    {
        maxHealth = _max;
        currentHealth = _max;
        
        // 재활용이기 때문에 생성된 다른 hud가 붙을 수 있다
        hpSlider = UIManager.Instance.ShowMultipleUI<HPSlider>(UIPanelType.hpSlider);
        hpSlider.SetTarget(transform);

        hpSlider.OnChangedValue(GetHealthRatio());
    }
    // 현재 체력 비율 (0~1)
    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }

    // 현재 체력 백분율 (0~100%)
    public float GetHealthPercentage()
    {
        return GetHealthRatio() * 100f;
    }

    // 소모된 체력 백분율 (0~100%)
    public float GetHealthLostPercentage()
    {
        return (1 - GetHealthRatio()) * 100f;
    }

    // 체력 변경 메서드
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Max(currentHealth - _damage, 0);
        hpSlider.OnChangedValue(GetHealthRatio());
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        hpSlider.OnChangedValue(GetHealthRatio());
    }
    private void Update()
    {
        hpSlider.FollowTarget();
    }
}
