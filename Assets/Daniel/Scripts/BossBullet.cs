using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private float time = 0;
    [SerializeField] private GameObject health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (collision.gameObject.tag == "Player")
        {
            health.GetComponent<Health>().TakeDamage(5);
        }
        Destroy(gameObject);
    }
}
