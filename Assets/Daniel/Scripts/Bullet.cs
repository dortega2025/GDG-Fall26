using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float time;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    bool TouchingDeath()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0f;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null) {
            return true;
        }
        else
        {
            hit = Physics2D.Raycast(position, direction, distance, enemyLayer);
            if (hit.collider != null) 
            {
            return true;
            }
            return false;
            
        }
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 3f)
        {
            Destroy(gameObject);
        }

        if (TouchingDeath())
        {
            Destroy(gameObject);
        }
    }
}
