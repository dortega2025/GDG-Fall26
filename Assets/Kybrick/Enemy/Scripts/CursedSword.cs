using UnityEngine;

public class CursedSword : MonoBehaviour
{
    [SerializeReference] private GameObject Particles;
    [SerializeField] private float RotSpd;
    [SerializeField] private float FlySpd;
    [SerializeField] private float AggroRange;
    [SerializeField] private float AtkRange;
    [SerializeField] private float AtkTimer;
    [SerializeField] private float HP;
    [SerializeReference] private Rigidbody2D RB;
    private GameObject Player;
    private float timer;
    private Vector3 DIR;
    private bool Attacking;
    private bool returning;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.Find("Player ");
    }

    public void Update()
    {
        if (Attacking)
        {
            timer -= Time.deltaTime;
            if (timer <= 1F&& timer >0.98F)
            {
                GameObject Wow = Instantiate(Particles);
                Wow.transform.position = transform.position;
                Wow.transform.parent = transform;
                Wow.transform.localPosition = new Vector3(0, -1.75F, 1);
                Wow.transform.localScale = new Vector3(2, 1, 2);
                Wow.transform.rotation = transform.rotation;
                DIR = Player.transform.position - transform.position;
            }
            else if (timer < 0)
            {
                float angle = Mathf.Atan2(DIR.y, DIR.x) * Mathf.Rad2Deg-90;
                angle = Mathf.Repeat(angle, 360);
                angle = angle - transform.rotation.z;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                RB.AddForce(DIR.normalized * Time.deltaTime * FlySpd, ForceMode2D.Impulse);
            }
            else
            {
                transform.Rotate(Vector3.forward, RotSpd);
            }
        }
        else if (Vector2.Distance(Player.transform.position, transform.position) <= AggroRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, (FlySpd / 3)*Time.deltaTime);
            if(Vector2.Distance(Player.transform.position, transform.position) <= AtkRange)
            {
                Attacking = true;
                timer = AtkTimer;
                returning = true;

            }
        }

        if(Attacking&& Vector2.Distance(Player.transform.position, transform.position) >= AggroRange)
        {
            if (returning)
            {
                RB.linearVelocity = new Vector2(0, 0);
                RB.angularVelocity = 0;
                returning = false;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, (FlySpd)*Time.deltaTime);
                if (Vector2.Distance(Player.transform.position, transform.position) <= AggroRange)
                {
                    Attacking = false;
                }
            }


        }
    }       
           
}
