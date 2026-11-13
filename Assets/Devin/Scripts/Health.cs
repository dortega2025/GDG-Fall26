using Unity.VisualScripting;
using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float InvulTime;
    [SerializeField] private float InvulTimeMax;
    [SerializeField] private float Def;


    public HealthBar healthBar;

    void Awake()
    {
      currentHealth = maxHealth; 
      healthBar.SetMaxHealth(maxHealth); 
    }

    public void TakeDamage(float damage)
    {
        if (InvulTime <= 0)
        {
            InvulTime = InvulTimeMax;

            currentHealth -= damage/Def;

            healthBar.SetHealth(currentHealth);
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void InitiateDeath()
    {
        //TODO: Eventually this should trigger a death sequence
    }
    public void Update()
    {
        if (InvulTime - Time.deltaTime >= 0)
        {
            InvulTime -= Time.deltaTime;
        }
        else
        {
            InvulTime = 0;
        }
    }

}
