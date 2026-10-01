using UnityEngine;

public class NodeMovementEnemy : MonoBehaviour
{
    [SerializeField] private float Invul;// amount of frames they are invulnerable after recieving damage
    [SerializeField] private float Hp;//How many times they can be hit 
    [SerializeField] private float Dmg; // how much damage it will do on contact 
    [SerializeField] private float AtkSpd;// how quickly it can attack
    [SerializeField] private float AggroRange;// how close the player has to be for them to aggro
    [SerializeField] private float ProjectileSpd;// how fast the pojectile goes (If ranged)
    [SerializeField] private float AtkRange;//how close the player has to be before it attacks
    [SerializeReference] private GameObject Player;
    [SerializeReference] private GameObject Weapon; // what attack they use 
    public float AtkTimer; 
    public bool Range = false;//are they a melee or are they a ranged enemy
    public LayerMask groundMask; // IDk i stole this from Daniel
    public float InvulTimer; // invulTImer, mainly to stop multihts
    public bool Aggrod;// if aggro, then do things!
    //Node movement Variables
    [SerializeReference] private GameObject NodeLU;//the left/up node
    [SerializeReference] private GameObject NodeRD;//the right/down node
    [SerializeReference] private GameObject Platform;//the platform that moves
    public float Speed;// speed of platform
    public float Dir;// direction of platform 1 == to LU; 2== to RD
    float Distance;//how close it is to next Nodule
    public float DistTolerance;//how close the platform can be before turning around



    private void OnCollisionEnter2D(Collision2D collision)
    {
    if (collision.gameObject.CompareTag("PBullet") && (InvulTimer == 0)) //check if invulnerable
    {
        InvulTimer = Invul;//Set timer
        Hp -= 1;//reduce hp, need to change this out to find away to get the damage value from the bullet itself
        Destroy(collision.gameObject);// destroy bullet to stop it from gunking anything up
        if (Hp <= 0)//Check here if dead, so we dont wait until no longer invul
        {
            Destroy(gameObject);//destroy this homeboy!
        }
    }
    else if (collision.gameObject.CompareTag("PBullet"))
    {
        Destroy(collision.gameObject);//destroys bullet regardless
    }
    }

    void Update()
    {
        if (InvulTimer - 1 * Time.deltaTime >= 0)//if InvulTimer is over 0
        {
            InvulTimer -= 1 * Time.deltaTime;//reduce it

        }
        else// if Time.deltaTime would be too much to put it directly at 0
        {
            InvulTimer = 0;//manually set it to 0 to avoid infinte InvulTime
        }

        if (AtkTimer - 1 * Time.deltaTime >= 0)
        {
            AtkTimer -= 1 * Time.deltaTime;
        }
        else
        {
            AtkTimer = 0;
        }
    }

    void FixedUpdate()
    {
        Aggro();

        if (Aggrod)
        {
            if (AtkTimer == 0)
            {
                if (Vector2.Distance(transform.position, Player.transform.position) <= AtkRange)
                {
                    Attack();
                    AtkTimer = AtkSpd;
                }
                else
                {
                    Move();
                }
            }

        }
        else
        {
            Move();
        }
        //I have two calls of the move function because I dont want this enemy to be moving along it's path when it is attacking
    }

    void Move()
    {
        if (Dir == 1)
        {
            Platform.transform.position = Vector2.MoveTowards(Platform.transform.position, NodeLU.transform.position, Speed * Time.deltaTime);
            //moving to LU
            Distance = Vector2.Distance(Platform.transform.position, NodeLU.transform.position);
            //Checking distanct to LU
        }
        else
        {
            Platform.transform.position = Vector2.MoveTowards(Platform.transform.position, NodeRD.transform.position, Speed * Time.deltaTime);
            //moving to RD
            Distance = Vector2.Distance(Platform.transform.position, NodeRD.transform.position);
            //Checking distance to RD
        }

        if (Distance <= DistTolerance)
        {
            if (Dir == 1)
            {
                Dir = 2;
                //if going to LU go then go to RD
            }
            else
            {
                Dir = 1;
                //else going to RD then go to LU
            }
        }
    }



    void Aggro()// if player is within a certain range, activate aggro
    {
        if(Vector2.Distance(transform.position,Player.transform.position)<= AggroRange)
        {
            Vector3 direction = Player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            angle = Mathf.Repeat(angle, 360);
            angle = angle - transform.rotation.z;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            RaycastHit2D GroundCheck = Physics2D.Raycast(transform.position,direction,AggroRange,groundMask);
            if(GroundCheck.collider == null)
            {
                Aggrod = true;
            }
            else
            {
                Aggrod = false;
            }
       }
    }

    void Attack()
    {
        //stolen code from Daniel :)
        
        GameObject Atk;
       
            Vector3 direction = Player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            angle = Mathf.Repeat(angle, 360);
            angle = angle - transform.rotation.z;
            Atk = Instantiate(Weapon, transform.position, Quaternion.Euler(0, 0, angle));
            if (Range == true)
            {
                Atk.GetComponent<Rigidbody2D>().AddForce(direction.normalized * ProjectileSpd, ForceMode2D.Impulse); //Movement projectile if ranged
            }
            else
            {
                Atk.transform.parent = transform;// move melee hitbox W/ enemy if melee
            }
        
    }


}
