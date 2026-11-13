using UnityEngine;

public class GraveMagician : MonoBehaviour
{
    [Header("Standard")]
    //[SerializeField] private;
    //[SerializeReference] private;
    [SerializeReference] private Health health;
    [SerializeReference] private PlayerInput PlayerI;
    [SerializeReference] private GameObject Player;
    [SerializeReference] private GameObject HealthLogic;

    [Header("Stats")]
    [SerializeField] private float HP;
    [SerializeField] private float Def;
    [SerializeField] private float AtkSpd;
    [SerializeField] private float BulletSpd;
    [SerializeField] private float AtkWindUp;
    [SerializeField] private float AtkRange;
    [SerializeField] private bool PlayerInRange=false;
    [SerializeField] private bool PlayerDirection;

    [Header("References")]
  
    [SerializeReference] private GameObject Top;
    [SerializeReference] private GameObject Bottom;
    [SerializeReference] private GameObject ATkOB;
    [SerializeReference] private GameObject ATKGen;
    [SerializeReference] private GameObject ATKWindUpParticles;
    private Vector3 TopRestingPos;
    private Vector3 TopWindUpPos;

    private Vector3 BottomRestPos;
    private Vector3 BottomWindUpPos;



    [Header("Timers")]

    [SerializeField] private float AtkTimer;
    [SerializeField] private float WindUpTimer;
    private bool ParticleOn = true;
    void Start()
    {
        AtkTimer = AtkSpd;
        WindUpTimer = AtkWindUp;
        BottomWindUpPos = Bottom.transform.localPosition;
        TopWindUpPos = Top.transform.localPosition;
        BottomRestPos = new Vector3(0, 0.461F, 0);
        TopRestingPos = new Vector3(0, -0.036F, 0);
        HealthLogic = GameObject.Find("HealthLogic");
        health = HealthLogic.GetComponent<Health>();
        Player = GameObject.Find("Player ");
        PlayerI = Player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    public void Update()
    {

        if (Vector2.Distance(ATKGen.transform.position,Player.transform.position)<=AtkRange) 
        {
            PlayerInRange = true;
            if(AtkTimer- 1*Time.deltaTime >= 0)
            {
                AtkTimer -= 1 * Time.deltaTime;
            }
            else
            {
                AtkTimer = 0;
            }

            if (AtkTimer <=0)
            {
                if(WindUpTimer-1*Time.deltaTime >= 0)
                {
                    if (ParticleOn)
                    {
                        ParticleOn = false;
                        GameObject ATKP = Instantiate(ATKWindUpParticles);
                        ATKP.transform.position = transform.position;
                    }
                    WindUpTimer -= 1 * Time.deltaTime;
                }
                else
                {
                    WindUpTimer = 0;
                }
                
            }

            if(WindUpTimer <= 0)
            {
                GameObject ATK = Instantiate(ATkOB); 
                ATK.transform.position = ATKGen.transform.position;
                Vector2 Aim = transform.position;
                Vector2 Dir;
                if (Player.transform.position.x <= transform.position.x)
                {
                    Aim.x += BulletSpd;
                    Dir = new Vector2(ATKGen.transform.position.x - Aim.x, 0);
                }
                else
                {
                    Aim.x -= BulletSpd;
                    Dir = new Vector2(ATKGen.transform.position.x - Aim.x,0);
                    ATK.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                AtkTimer = AtkSpd;
                ParticleOn = true;
                WindUpTimer = AtkWindUp;
                ATK.GetComponent<Rigidbody2D>().AddForce(Dir.normalized * BulletSpd * Time.deltaTime, ForceMode2D.Impulse);
            }
        }
        else
        {
            PlayerInRange = false;
        }
        if (Top.transform.position != TopRestingPos && !PlayerInRange&&ParticleOn)
        {
            Top.transform.localPosition = Vector2.MoveTowards(Top.transform.localPosition, TopRestingPos, 10 * Time.deltaTime);

        }else if(Top.transform.position!=TopWindUpPos && PlayerInRange)
        {
            Top.transform.localPosition = Vector2.MoveTowards(Top.transform.localPosition, TopWindUpPos, 10 * Time.deltaTime);
        }
        if(Bottom.transform.position!= BottomRestPos && !PlayerInRange&&ParticleOn)
        {
            Bottom.transform.localPosition = Vector2.MoveTowards(Bottom.transform.localPosition, BottomRestPos, 10 * Time.deltaTime);
        }else if(Bottom.transform.position != BottomWindUpPos && PlayerInRange)
        {
            Bottom.transform.localPosition = Vector2.MoveTowards(Bottom.transform.localPosition, BottomWindUpPos, 10 * Time.deltaTime);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PBullet"))
        {
            HP -= (PlayerI.Atk / Def);
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
