using UnityEngine;

public class GreenFlameNode : MonoBehaviour
{
    [SerializeReference] private GameObject Gen00;
    [SerializeReference] private GameObject Gen;
    [SerializeReference] private Rigidbody2D RB;
    [SerializeReference] private BoxCollider2D Box;

    [SerializeField] private float speed;
    [SerializeField] private float LifeSpan;
    [SerializeField] private float GravCheck;
    [SerializeReference] private Health health;
    [SerializeReference] private GameObject HealthLogic;
    [SerializeReference] private GameObject Player;

    public int RandomMax;
    private int RNG;
    bool on = true;
    private Vector3 Dir;
    void Start()
    {
        RNG = Random.Range(-RandomMax, RandomMax);
        speed += RNG;
        Player = GameObject.Find("Player ");
        HealthLogic = GameObject.Find("HealthLogic");
        health = HealthLogic.GetComponent<Health>();
    }
    void Update()
    {

        LifeSpan -= Time.deltaTime;
        if (LifeSpan < GravCheck)
        {
            Box.isTrigger = true;
            RB.gravityScale = 0;
            RB.angularVelocity = 0;
            RB.linearVelocity = new Vector2(0,0);
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (LifeSpan < 0)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            Debug.Log("Heya You got hit by resting node");
            health.TakeDamage(5);
            Destroy(gameObject);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player)
        {
            Debug.Log("Heya You got hit by flying node");
            health.TakeDamage(10);
            Destroy(gameObject);
        }
    }

}
