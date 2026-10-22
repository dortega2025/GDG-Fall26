using Unity.VisualScripting;
using UnityEngine;

public class BulletsKybrick : MonoBehaviour
{ 
[SerializeField] private float time;
[SerializeReference] private Rigidbody2D rb;
[SerializeReference] private Collider2D collider;
// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
{
    rb = GetComponent<Rigidbody2D>();
    collider = GetComponent<CapsuleCollider2D>();
}
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
    }
    else
    {
            if (collision.gameObject.CompareTag("Enemy"))
            {
              
            }
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

public void Move(Vector2 direction, float Speed)
{
    rb.AddForce(direction.normalized * (1f * Speed), ForceMode2D.Impulse);
}
void OnTriggerEnter2D(Collider2D collision)
{
    Destroy(gameObject);
}
}
