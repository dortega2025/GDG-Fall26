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
                GameObject Atk = Bullet;
                Vector2 direction = Square.transform.position - Triangle.transform.position;
                Instantiate(Atk, transform.position, transform.rotation);
                Atk.GetComponent<Rigidbody2D>().AddForce( direction.normalized* BulletSpd, ForceMode2D.Impulse); 
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
