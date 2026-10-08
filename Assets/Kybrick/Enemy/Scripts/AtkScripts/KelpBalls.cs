using UnityEngine;

public class KelpBalls : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float Lifespan;
    private GameObject player;
    private float Life;
    private Vector2 StartPos;
    private Vector2 TargetPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        StartPos = transform.position;
        TargetPos = player.transform.position;
        Vector2 direction = StartPos - TargetPos;
        rb.AddForce(-direction.normalized * (1f*Speed), ForceMode2D.Impulse);
    }
    private void Update()
    {
        Life += 1 * Time.deltaTime;
        if (Life > Lifespan)
        {
            Destroy(gameObject);
        }
    }
}
