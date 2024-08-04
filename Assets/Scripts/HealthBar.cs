using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Text healthText; // Text component to display health
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        InitializeHealthBar(maxHealth);
    }

    private void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void InitializeHealthBar(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void UpdateHealthBar()
    {
        // Update health text
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}";
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void RemoveHealth(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }
}
