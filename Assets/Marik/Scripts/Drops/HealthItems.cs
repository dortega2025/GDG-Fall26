using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] public bool usePercentage;
    [SerializeField] public float heathValue;

    [SerializeField] private GameObject pickUpSound;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(pickUpSound);
            GameObject healthLogic = GameObject.Find("HealthLogic");
            Health health = healthLogic.GetComponent<Health>();
            if (usePercentage)
            {
                Debug.Log("Percent health restore is TODO");
            }
            else
            {
                health.TakeDamage(-heathValue);
            }
            Destroy(gameObject);
        }
    }
}
