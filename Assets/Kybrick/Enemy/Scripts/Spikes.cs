using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeReference] private Health health;
    [SerializeReference] private Rigidbody2D Rb;
    [SerializeReference] private GameObject HealthLogic;
    [SerializeReference] private GameObject Player;

    public void Start()
    {
        HealthLogic = GameObject.Find("HealthLogic");
        Player = GameObject.Find("Player ");
        health = HealthLogic.GetComponent<Health>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player)
        {
            health.TakeDamage(10);
        }
    }

}
