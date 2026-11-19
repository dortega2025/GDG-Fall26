using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class ItemDropSpawner : MonoBehaviour

{
    [Header("Healt Drop")]
    [SerializeField] private GameObject healthDrop;
    [SerializeField] private bool healthGainByPercent;
    [SerializeField] private int minHealthDrop = 5;
    [SerializeField] private int maxHealthDrop = 25;

    [Header("Drop Chances")]
    [SerializeField] private int chanceToDropHealth = 5;

    [Header("Other")]
    [SerializeField] private bool instantDestroy;
    [SerializeField] private bool onlyDropOneItem;
    void Start()
    {
        if (instantDestroy)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        bool happened;
        happened = DropHealth();
        if (happened && onlyDropOneItem) {return;}

    }

    private bool DropHealth()
    {
        if (UnityEngine.Random.Range(-1, 99) < chanceToDropHealth)
        {
            GameObject drop = Instantiate(healthDrop);
            HealthItem scr = drop.GetComponent<HealthItem>();
            scr.usePercentage = healthGainByPercent;
            scr.heathValue = UnityEngine.Random.Range(minHealthDrop, maxHealthDrop);
            return true;
        }
        return false;
    }
}
