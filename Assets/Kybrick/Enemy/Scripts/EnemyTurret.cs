using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTurret : MonoBehaviour
{
    [SerializeReference] private GameObject Bullet;
    [SerializeReference] private GameObject Square;
    [SerializeReference] private GameObject Triangle;
    [SerializeReference] private GameObject Generator;
    [SerializeReference] private GameObject Player;
    public float AtkSpd;
    public float BulletSpd;
    float AtkTimer;
    public float AggroRange;
    bool On;
    private void Start()
    {
        //Player = GameObject.Find("Player");
    }
    private void Update()
    {
        if (On)
        {
            if(AtkTimer - 1 *Time.deltaTime >= 0)
            {
                AtkTimer -= 1 *Time.deltaTime;
            }
            else
            {
                AtkTimer = AtkSpd;
                GameObject Atk;
                Vector3 direction = Square.transform.position - Triangle.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-270;
                angle = Mathf.Repeat(angle, 360);
                angle = angle - transform.rotation.z;
                direction.z = 0;
                direction = direction.normalized;
                Atk = Instantiate(Bullet, Generator.transform.position, Quaternion.Euler(0, 0, angle));
                Atk.GetComponent<Rigidbody2D>().AddForce((direction * BulletSpd)*-1, ForceMode2D.Impulse); 
            }
        }
            CheckDistance();        
    }
    void CheckDistance()
    {
        if (Vector2.Distance(transform.position,Player.transform.position)<=AggroRange)
        {
            On = true;
        }
        else
        {
            On = false;
        }
    }
}
