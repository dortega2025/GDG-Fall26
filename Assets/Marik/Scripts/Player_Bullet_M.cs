using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class P_Bullet_M : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider;
    [SerializeField] public float damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colderTag = collision.gameObject.tag;
        if (colderTag == "Player")
        {
            
        }
        else if (LayerMask.LayerToName(collision.gameObject.layer) == "Enemies")
        {
            /*
            enemyTakeDamage damageScript = collision.gameObject.GetComponent<enemyTakeDamage>();
            damageScript.TakeDamage(damage);
            */
            Destroy(gameObject);
        }
        else if (colderTag == "Platform")
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
        
        if (time <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            time -= 1 * Time.deltaTime;
        }
    }

    public void Move(Vector2 direction,float Speed)
    {
        rb.AddForce(direction.normalized * (1f * Speed), ForceMode2D.Impulse);
    }

}
