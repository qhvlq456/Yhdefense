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
        
        // ��Ȱ���̱� ������ ������ �ٸ� hud�� ���� �� �ִ�
        hpSlider = UIManager.Instance.ShowMultipleUI<HPSlider>(UIPanelType.hpSlider);
        hpSlider.SetTarget(transform);

        hpSlider.OnChangedValue(GetHealthRatio());
    }
    // ���� ü�� ���� (0~1)
    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }

    // ���� ü�� ����� (0~100%)
    public float GetHealthPercentage()
    {
        return GetHealthRatio() * 100f;
    }

    // �Ҹ�� ü�� ����� (0~100%)
    public float GetHealthLostPercentage()
    {
        return (1 - GetHealthRatio()) * 100f;
    }

    // ü�� ���� �޼���
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
