using Unity.VisualScripting;
using UnityEngine;

public class Tangle : MonoBehaviour
{
    [SerializeReference] private Rigidbody2D rb;
    [SerializeField] private float Spd;
    [SerializeField] private float AggroRge;
    [SerializeField] private float AtkRge;
    [SerializeField] private float AtkSpd;
    [SerializeField] private float DashWindUp;
    [SerializeField] private float DashRge;
    [SerializeField] private float DashSpd;
    [SerializeField] private float Hp;
    public bool Aggro;
    private bool AtkAble;
    private Vector2 TargetPos;
    private Vector2 CurrentPos;
    private float Timer;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        AtkAble = true;
    }
    void Update()
    {
        if(Timer - 1 * Time.deltaTime>=0)
        {
            Timer -= 1 * Time.deltaTime;
        }
        else if(AtkAble==false)
        {
            AtkAble = true;
            Timer = 0;
            Dash();
        }
        if (Vector2.Distance(transform.position, player.transform.position) <= DashRge&&AtkAble)
        {
            AtkAble = false;
            LockOn();
        }else if (LocatePlayer())
        {
            Move();
        }
    }
    void Move()
    {
        CurrentPos = transform.position;
        TargetPos = player.transform.position;
        rb.transform.position = Vector2.MoveTowards(CurrentPos, TargetPos, Spd);
    }
    bool LocatePlayer()
    {
        bool on;
        if (Vector2.Distance(transform.position,player.transform.position)<= AggroRge)
        {
            on = true;
        }
        else
        {
            on = false;
        }
        return on;
    }
    public void TakeDamage(float damage)
    {
        Hp-=damage;
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    void LockOn()
    {
        CurrentPos = transform.position;
        TargetPos = player.transform.position;
        Timer = DashWindUp;
    }
    void Dash()
    {
        Vector2 direction= CurrentPos - TargetPos;
        rb.AddForce(direction.normalized * DashSpd, ForceMode2D.Impulse);
    }
}
