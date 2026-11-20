using UnityEngine;

public class IndividualLaser : MonoBehaviour
{
    [SerializeField] private GameObject health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GameObject.Find("HealthLogic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        health.GetComponent<Health>().TakeDamage(5f);
    }
}
