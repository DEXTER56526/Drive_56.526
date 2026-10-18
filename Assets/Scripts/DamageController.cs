using UnityEngine;

// Simple damage model for vehicles. You can expand to visual damage and physics modifiers.
public class DamageController : MonoBehaviour
{
    [Header("Damage")]
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount)
    {
        currentHealth = Mathf.Max(0f, currentHealth - amount);
        // Optionally: modify performance when damaged
    }

    public void ResetDamage()
    {
        currentHealth = maxHealth;
        Debug.Log($"{name}: Damage reset to full.");
    }

    public bool IsDestroyedOrDisabled()
    {
        return currentHealth <= 0f;
    }
}
