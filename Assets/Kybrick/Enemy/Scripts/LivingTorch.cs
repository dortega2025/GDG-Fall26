using UnityEngine;

public class LivingTorch : MonoBehaviour
{
    [Header("Standard")]
    [SerializeReference] private Health health;
    [SerializeReference] private PlayerInput PlayerI;
    [SerializeReference] private GameObject Player;
    [SerializeReference] private GameObject HealthLogic;
    [SerializeReference] private SpriteRenderer Sprites;
    [SerializeReference] private Rigidbody2D RB;

    [Header("Both")]
    [SerializeField] private float HP;
    [Header("Hidden")]
    [SerializeField] private float HDef;
    [SerializeField] private float HAtk;
    [SerializeField] private float HAggroRange;
    [SerializeReference] private Sprite HSprite;

    [Header("Awake")]
    [SerializeField] private bool Awake;
    [SerializeField] private float AAtk;
    [SerializeField] private float ADef;
    [SerializeField] private float AAggroRange;
    [SerializeField] private float ASpd;
    [SerializeField] private bool AAgro;
    [SerializeReference] private Sprite ASprite;

    void Start()
    {
        Player = GameObject.Find("Player ");
        PlayerI = Player.GetComponent<PlayerInput>();
        HealthLogic = GameObject.Find("HealthLogic");
        health = HealthLogic.GetComponent<Health>();
        Awake = false;
        Sprites.sprite = HSprite;
    }

    void Update()
    {
        CheckRange();
        if (Awake)
        {
            RB.freezeRotation = false;
            Vector2 Dir = transform.position - Player.transform.position;
            float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg - 90;
            angle = Mathf.Repeat(angle, 360);
            angle = angle - transform.rotation.z;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, ASpd*Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            RB.freezeRotation = true;
            RB.angularVelocity = 0;
            RB.linearVelocity = new Vector2(0, 0);
        }
    }
    public void CheckRange()
    {
        if (Awake)
        {
            if(Vector2.Distance(Player.transform.position,transform.position)<= AAggroRange)
            {

            }
            else
            {
                Awake = false;
                Sprites.sprite = HSprite;

            }
        }
        else if (Vector2.Distance(Player.transform.position, transform.position) <= HAggroRange)
        {
            Awake = true;
            Sprites.sprite = ASprite;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PBullet"))
        {
            if (Awake)
            {
                HP -= PlayerI.Atk / ADef;
            }
            else
            {
                HP -= PlayerI.Atk / HDef;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Awake = true;
                Sprites.sprite = ASprite;
                RB.freezeRotation = false;
                RB.angularVelocity = 0;
                RB.linearVelocity = new Vector2(0, 0);
            }
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }
        if(collision.gameObject == Player)
        {
            if (Awake)
            {
                health.TakeDamage(AAtk);
            }
            else
            {
                health.TakeDamage(HAtk);
            }           
        }
    }
}
