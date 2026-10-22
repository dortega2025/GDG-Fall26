using System;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    /*
    This script only ever affects the Health Bar UI and its components. 
    The actual handling of the players health is dealth with in the Health Script 
    and PlayerInput Script. The Health Script you should be able to just
    put into an empty game object. The code in the PlayerInput script is
    temporary, but is currently required to simulate damage.
    */

    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    public void SetMaxHealth (float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);

    }

}

