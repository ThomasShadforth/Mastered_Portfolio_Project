using System;

public class HealthSystem
{
    int _currentHealth;
    int _maxHealth;

    public event EventHandler OnHealthChanged;

    public int GetHealth()
    {
        return _currentHealth;
    }

    public void UpdateMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;
    }

    public float GetHealthPercent()
    {
        return (float)(_currentHealth / _maxHealth);
    }

    public void Damage(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
        }

        OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;

        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        OnHealthChanged(this, EventArgs.Empty);
    }

    public bool CheckIsDead()
    {
        return _currentHealth <= 0;
    }

}
