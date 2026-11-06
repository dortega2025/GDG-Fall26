using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float time = 0;
    [SerializeReference] private Rigidbody2D rb;
    [SerializeReference] private Collider2D collider;
    [SerializeField] private GameObject health;
    public float damage = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        health = GameObject.Find("HealthLogic");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //health.GetComponent<Health>().TakeDamage(5);
        Destroy(gameObject);
    }
}
