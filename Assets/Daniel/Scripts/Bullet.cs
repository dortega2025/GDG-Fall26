using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float time;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 3f)
        {
            Destroy(gameObject);
        }
    }
}
