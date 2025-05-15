using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth { get; private set; }
    public float maxHealth { get; private set; }
    public void ResetHealth(float _max)
    {
        maxHealth = _max;
        currentHealth = _max;
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
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
