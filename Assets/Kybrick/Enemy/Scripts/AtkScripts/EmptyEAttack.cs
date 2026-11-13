using UnityEngine;

public class EmptyEAttack : MonoBehaviour
{
    [SerializeReference] private Health health;
    [SerializeReference] private GameObject HealthLogic;
    [SerializeReference] private GameObject Player;
    [SerializeField] private float Dmg;
    void Start()
    {
        Player = GameObject.Find("Player ");
        HealthLogic = GameObject.Find("HealthLogic");
        health = HealthLogic.GetComponent<Health>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            health.TakeDamage(Dmg);
            Destroy(gameObject);
        }
    }

}