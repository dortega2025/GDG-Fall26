using Unity.VisualScripting;
using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    public HealthBar healthBar;

    void Awake()
    {
      currentHealth = maxHealth; 
      healthBar.SetMaxHealth(maxHealth); 
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void InitiateDeath()
    {
        //TODO: Eventually this should trigger a death sequence
    }

}
