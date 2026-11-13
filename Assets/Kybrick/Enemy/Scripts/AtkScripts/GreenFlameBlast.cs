using UnityEngine;

public class GreenFlameBlast : MonoBehaviour
{
    [SerializeReference] private Health health;
    [SerializeReference] private GameObject HealthLogic;
    [SerializeReference] private GameObject Player;

    public float die;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.Find("Player ");
        HealthLogic = GameObject.Find("HealthLogic");
        health = HealthLogic.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        die += Time.deltaTime;
        if (die > 10)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            Debug.Log("Heya You got hit by blast");
            health.TakeDamage(10);
        }
    }
}
