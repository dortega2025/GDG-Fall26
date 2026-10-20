using System;
using System.Drawing;
using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Small_Enemy_Basic_AI : MonoBehaviour
{
    public enum BulletFireType {Standered, Lob}; //TODO

    [Header("Connections")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rigBod;
    [SerializeField] private GameObject PrefabBullet;
    ///////////////////////////////////////////////////////////
    ///Only give a prefab gun, or a already made gun
    [SerializeField] private GameObject gun; //Takes priority
    [SerializeField] private GameObject prefabGun; 
    ////////////////////////////////////////////////////////////
    [SerializeField] private LayerMask groundMask;

    [Header("Visual Range Connections")]
    [SerializeField] private GameObject patrolPointLeft;
    [SerializeField] private GameObject patrolPointRight;
    [SerializeField] private GameObject agroRangeVisual;
    [SerializeField] private GameObject outerAttkRangeVisual;
    [SerializeField] private GameObject innerAttkRangeVisual;

    [Header("General Behavior")]
    [SerializeField] private bool hyperAgro; //If true, once agroed, won't de-agro
    //[SerializeField] bool canJump;
    [SerializeField] private bool isSmart; //If true, won't fall in pits
    [SerializeField] private bool flys; //TODO Everything
    [SerializeField] private bool ignorCollision; //TODO 
    [SerializeField] private bool solidToPlayer; //TODO
    [SerializeField] private bool lockDamage; //Forces each hit to this enemy deal one damage
    [SerializeField] private bool chasePlayer; //Can attack player but continues patrol

    [Header("General Stats")]
    [SerializeField] private float health;
    [SerializeField] private int invincibilityTime;

    [Header("Attack")] 
    [SerializeField] private float touchDamage;
    [SerializeField] private int projectilesPerFire; //TODO //The number of projectiles launched at a single fire
    [SerializeField] private int projectilesPerBurst; //TODO //How many times does the weopon fire in succstion
    [SerializeField] private float timeBetweenBurstFiredProjectiles; //Bigger number, the longer it takes
    [SerializeField] private bool chargeAttack; //Means once player enters attack range, enemy will wait firerate befor firing
    [SerializeField] private int fireRate; //Bigger the number, lower the fire rate
    [SerializeField] private float bulletSpread; //In degrees, maximum possible deviation from attempted target

    [Header("Projectile Behavior")]
    [SerializeField] private bool specialRangedAttack; //When turned on, ... All below "Projectile Behavior" options will be ignored, a spetialized bullet prefab should be used
    [SerializeField] private float bulletLifeSpan; //TODO
    [SerializeField] private float bulletVelocity; // TODO
    [SerializeField] private float gravitationalEffect; //TODO
    [SerializeField] private float bulletHomingPower; //TODO
    [SerializeField] private BulletFireType bulletFireType; //TODO

    [Header("Projectile Collisions")]
    [SerializeField] private bool bulletsDestroyedByObstacles; //TODO
    [SerializeField] private bool projectileFriendlyFire; //TODO
    [SerializeField] private bool projectileCollidesWithFriendly; //TODO
    [SerializeField] private bool projectileCollidesWithFriendlyBullets; //TODO
    [SerializeField] private bool projectileCollidesWithPlayerBullets; //TODO

    [Header("Projectile Explotion")]
    [SerializeField] private bool projectileExplodesOnDestruction; //TODO
    [SerializeField] private float projectileExplotionDamage; //TODO
    [SerializeField] private float projectileExplotionRange; //TODO


    [Header("Attack Range")]
    [SerializeField] private float agroMoveSpeed;
    [SerializeField] private bool useVisualAgroRanges;
    [SerializeField] private float agroRange;
    [SerializeField] private bool useVisualAttackRanges;
    [SerializeField] private float attackRangeOuter;
    [SerializeField] private float attackRangeInner;

    [Header("Final Blast")] //TODO Everything
    [SerializeField] private bool finalBlast;
    [SerializeField] private float earlyBlastSetoffRange;
    [SerializeField] private float finalBlastDamage;
    [SerializeField] private float finalBlastRadius;
    [SerializeField] private float finalBlastForce;

    [Header("Patrol")]
    [SerializeField] private bool useVisualPatrolPoints;
    [SerializeField] private bool beginPatrolRight;
    [SerializeField] private bool resetPatrolRangeWhenDeAgroed; //If true, when de-agroed, patrol points will be reset in relation to current position
    [SerializeField] private float patrolMoveSpeed;
    [SerializeField] private float patrolRangeLeft;
    [SerializeField] private float patrolRangeRight;
    [SerializeField] private float patrolRangeLeftY;
    [SerializeField] private float patrolRangeRightY;
    private bool neverAgroed = true;
    private bool justAgroed = false;
    private Vector3 rightPatrolPoint;
    private Vector3 leftPatrolPoint;
    private bool movingToRightPatrol;
    private int timeToVincibility = 0;
    private int timeTillShoot = 0;
    private bool noWeopon = false;
    
    void Start()
    {
        if (flys) {rigBod.gravityScale = 0;}

        CheckForVisualRanges();

        InitializePartol();

        if (prefabGun != null)
        {
            gun = Instantiate(prefabGun, transform.position, transform.rotation);
        }
        gun.transform.parent = gameObject.transform;
        if (gun == null)
        {
            noWeopon = true;
            Debug.Log("No weopon equipt!!!");
        }


    }

    void FixedUpdate()
    {
        GeneralNumbersUpdate();

        WeoponRotationControl();

        ///Movement
        if ((GetDistance() < agroRange) || (!neverAgroed && hyperAgro))
        {
            //Agroed
            int attackCheck;
            neverAgroed = false;
            justAgroed = true;

            if (flys) {attackCheck = AgroFly();} else {attackCheck = AgroMove();}

            if (!chasePlayer)
            {
                rigBod.linearVelocity = new Vector2(0, 0);
                if (flys) {FlyToPatrolPoint();} else {MoveToPatrolPoint();}
            }

            if (attackCheck == 0)
            {
                Attack();
            }
            else
            {
                if (chargeAttack)
                {
                    timeTillShoot = fireRate;
                }
            }
        }
        else
        {
            //Not Agroed (Patrol)  
            if (justAgroed)
            {
                justAgroed = false;
                if (resetPatrolRangeWhenDeAgroed)
                {
                    SetPatrolPoints();
                }
            }

            if (flys) {FlyToPatrolPoint();} else {MoveToPatrolPoint();}
        }

        AnimateSelf();

    }

    void OnDestroy()
    {
        if (finalBlast && GetDistance() <= finalBlastRadius)
        {
            //TODO add final blast damage
        }
    }
    
    private void CheckForVisualRanges()
    {
        if (useVisualPatrolPoints)
        {
            patrolRangeLeft = Math.Abs(patrolPointLeft.transform.position.x - transform.position.x);
            patrolRangeRight = Math.Abs(patrolPointRight.transform.position.x - transform.position.x);
            patrolRangeLeftY = patrolPointLeft.transform.position.y - transform.position.y;
            patrolRangeRightY = patrolPointRight.transform.position.y - transform.position.y;
        }
        Destroy(patrolPointLeft); Destroy(patrolPointRight);

        if (useVisualAgroRanges)
        {
            agroRange = Vector3.Distance(transform.position, agroRangeVisual.transform.position);
        }
        Destroy(agroRangeVisual);

        if (useVisualAttackRanges)
        {
            attackRangeOuter = Vector3.Distance(transform.position, outerAttkRangeVisual.transform.position);
            attackRangeInner = Vector3.Distance(transform.position, innerAttkRangeVisual.transform.position);
        }
        Destroy(outerAttkRangeVisual); Destroy(innerAttkRangeVisual);

        //Check if attack ranges make sense.
        if (attackRangeOuter > agroRange)
        {
            Debug.Log("Warning!!! Outer attack range is greater than agro range!!!");
        }
        if (attackRangeInner > attackRangeOuter)
        {
            Debug.Log("Warning!!! Inner attack range is greater than outer range!!!");
        }

    }
    
    private void GeneralNumbersUpdate()
    {
        if (timeToVincibility >= 0)
        {
            timeToVincibility -= 1;
        }

        if (!chargeAttack && timeTillShoot >= 0)
        {
            timeTillShoot -=1;
        }
    }
    
    private void Attack()
    {
        if (noWeopon) {return;}
        if (chargeAttack)
        {
            timeTillShoot -= 1;
        }

        if (timeTillShoot <= 0)
        {
            for (int i = 0; i < projectilesPerFire; i++)
            {
                CreateProjectile();
            }
            timeTillShoot = fireRate;
        }
    }

    private void CreateProjectile()
    {
        Debug.Log("Attempting bullet");
        {
            
        }
        float spread = bulletSpread/2;
        spread = UnityEngine.Random.Range(-spread, spread);

        Quaternion spawnAngle = Quaternion.AngleAxis(spread, Vector3.forward).normalized;

        GameObject newBullet = Instantiate(PrefabBullet, transform.position, spawnAngle);
        BasicEnemyBullet script = newBullet.GetComponent<BasicEnemyBullet>();

        Vector3 direction = spawnAngle * Vector3.right;
        newBullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletVelocity, ForceMode2D.Impulse);

        script.player = player;

        script.lifeSpan = bulletLifeSpan;
        script.velocity = bulletVelocity;
        script.gravitationalEffect = gravitationalEffect;
        script.homingPower = bulletHomingPower;
        script.destroyedByObstacles = bulletsDestroyedByObstacles;
        script.friendlyFire = projectileFriendlyFire;
        script.collidesWithFriendly = projectileCollidesWithFriendly;
        script.collidesWithFriendlyBullets = projectileCollidesWithFriendlyBullets;
        script.collidesWithPlayerBullets = projectileCollidesWithPlayerBullets;
    }
    
    private float GetDistance()
    {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceFromPlayer;
    }

    private int AgroMove()
    {
        if ((GetDistance() < attackRangeOuter) && (GetDistance() > attackRangeInner))
        {
            rigBod.linearVelocityX = 0;
            return 0; //Attack
        }
        else if (!CheckGrounded())
        {
            return -156;
        }
        else if (GetDistance() > attackRangeOuter)
        {
            if (chasePlayer)
            {
                //Get closer to player
                if (player.transform.position.x < transform.position.x)
                {
                    if (IsGroundAtNextPositon("Left"))
                    {
                        rigBod.linearVelocityX = -agroMoveSpeed;
                    }
                }
                else
                {
                    if (IsGroundAtNextPositon("Right"))
                    {
                        rigBod.linearVelocityX = agroMoveSpeed;
                    }
                }
                return 1;
            }
            else
            {
                return -911;
            }
        }
        else if (GetDistance() < attackRangeInner)
        {
            //Get farther to player
            if (player.transform.position.x < transform.position.x)
            {
                rigBod.linearVelocityX = agroMoveSpeed;
            }
            else
            {
                rigBod.linearVelocityX = -agroMoveSpeed;
            }
            return -1;
        }
        return -999;
    }

    private int AgroFly()
    {
        if ((GetDistance() < attackRangeOuter) && (GetDistance() > attackRangeInner))
        {
            return 0; //Attack
        }
        else
        {
            return 0;
        }
    }

    private void FlyToPatrolPoint()
    {
        if (movingToRightPatrol)
        {
            if (Vector3.Distance(rightPatrolPoint, transform.position) < 3)
            {//At patrol point right
                movingToRightPatrol = false;
            }
            else
            {//Move to patrol point right
                Vector3 direction = rightPatrolPoint - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                angle = Mathf.Repeat(angle, 360);
                angle -= transform.rotation.z;
                 //= new Vector3(0, 0, angle);
                
            }
        }
        else//Moving to left patrol
        {
            if (Vector3.Distance(rightPatrolPoint, transform.position) < 3)
            {//At patrol point left
                movingToRightPatrol = true;
            }
            else
            {//Move to patrol left right
                
            }
        }
    }
    
    private void MoveToPatrolPoint()
    {
        if (!CheckGrounded())
        {
            return;
        }
        if (movingToRightPatrol)
        {
            if ((rightPatrolPoint[0] < transform.position.x) || (!IsGroundAtNextPositon("Right")) || IsWallAtPosition("Right"))
            {//Reverses direction if past right patrol point
                movingToRightPatrol = false; 
            }
            else
            {
                rigBod.linearVelocityX = patrolMoveSpeed;
            }
        }
        else
        {
            if ((leftPatrolPoint[0] > transform.position.x) || (!IsGroundAtNextPositon("Left")) || IsWallAtPosition("Left"))
            {//Reverses direction if past left patrol point
                movingToRightPatrol = true;
            }
            else
            {
                rigBod.linearVelocityX = -patrolMoveSpeed;
            }
        }
    }

    private void SetPatrolPoints()
    {
        if (!flys)
        {
             leftPatrolPoint = new Vector3(transform.position.x - patrolRangeLeft, transform.position.y, 0);
             rightPatrolPoint = new Vector3(transform.position.x + patrolRangeRight, transform.position.y, 0);
        }
        else
        {
             leftPatrolPoint = new Vector3(transform.position.x - patrolRangeLeft, transform.position.y + patrolRangeLeftY, 0);
             rightPatrolPoint = new Vector3(transform.position.x + transform.position.y, transform.position.y + patrolRangeRightY, 0);
        }
        Debug.Log("Patrol points set:");
        Debug.Log("Left:"); Debug.Log(leftPatrolPoint);
        Debug.Log("Right:"); Debug.Log(rightPatrolPoint);
    }

    private void InitializePartol()
    {
        SetPatrolPoints();
        if (beginPatrolRight)
        {
            movingToRightPatrol = true;
        }
    }
    
    private bool CheckGrounded()
    {
        Vector2 position = transform.position;
        position.x -= (GetComponent<Renderer>().bounds.size.x)/2f;
        Vector2 direction = Vector2.down;
        float distance = 1f;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundMask);
        if (hit.collider != null) {
            position.x += (GetComponent<Renderer>().bounds.size.x);
            if (hit.collider != null)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private bool IsWallAtPosition(string Direction)
    {
        Vector3 sizeVect = GetComponent<Renderer>().bounds.size;
        float height = sizeVect.y;
        float width = sizeVect.x;
        Vector2 position;

        if (Direction == "Right")
        {
            position = new Vector2(transform.position.x + width/2f + .02f, transform.position.y + height/2f + .01f);
        }
        else
        {
            position = new Vector2(transform.position.x - width/2f - .02f, transform.position.y + height/2f + .01f);
        }
        
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, height - .02f, groundMask);
        if (hit.collider != null)
        {
            Debug.Log("Is wall");
            return true;  
        }
        else
        {
            return false;
        }
    }

    private bool IsGroundAtNextPositon(string Direction)
    {
        if (!isSmart)
        {
            return true;
        }
        Vector3 sizeVect = GetComponent<Renderer>().bounds.size;
        float height = sizeVect.y + 1;
        float width = (sizeVect.x / 2.0f) + .1f;
        Vector2 position;
        if (Direction == "Right")
        {
            position = new Vector2(transform.position.x + width, transform.position.y);
        }
        else
        {
            position = new Vector2(transform.position.x - width, transform.position.y);
        }
        
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, height, groundMask);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void WeoponRotationControl()
    {
        switch (bulletFireType)
        {
            case BulletFireType.Standered:
                float playerPosX = player.transform.position.x;
                float playerPosY = player.transform.position.y;
                Vector3 playerPos = new Vector3(playerPosX, playerPosY, 0f);
                Vector3 direction = playerPos - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                angle = Mathf.Repeat(angle, 360);
                angle -= transform.rotation.z;
                gun.transform.eulerAngles = new Vector3(0, 0, angle+180);
            break;
        } 
    }

    private void AnimateSelf()
    {
        
    }
    
    public void TakeDamage(float DMG)
    {
        if (timeToVincibility > 0)
        {//Still invinvible
            return;
        }
        else
        {
            timeToVincibility = invincibilityTime;
        }

        if (lockDamage)
        {
            health--;
        }
        else
        {
            health -= DMG;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


}