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
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Small_Enemy_Basic_AI : MonoBehaviour
{
    public enum BulletFireTypes {Standered, Lob, Special};
    public enum HomeingTypes {Nudge, Absolute, Turn};

    [Header("Connections")]
    private GameObject player;
    private Rigidbody2D rigBod;
    private Collider2D MainCollider;
    private SpriteRenderer spriteRenderer;
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
    //[SerializeField] bool canJump; //TODO
    [SerializeField] private bool isSmart; //If true, won't fall in pits
    [SerializeField] private bool flys; 
    [SerializeField] private bool activeAgroFlight; 
    [SerializeField] private bool ignorAllCollision; //TODO 
    //[SerializeField] private bool solidToPlayer; //TODO
    [SerializeField] private bool lockDamage; //Forces each hit to this enemy deal one damage
    [SerializeField] private bool chasePlayer; //Can attack player but continues patrol

    [Header("General Stats")]
    [SerializeField] private float health;
    [SerializeField] private int invincibilityTime;

    [Header("Attack")] 
    [SerializeField] private float touchDamage;
    [SerializeField] private float bulletDamage;
    [SerializeField] private int projectilesPerFire; //The number of projectiles launched at a single fire
    [SerializeField] private int projectilesPerBurst; //How many times does the weopon fire in succstion
    [SerializeField] private float timeBetweenBurstFiredProjectiles; //Bigger number, the longer it takes
    [SerializeField] private bool chargeAttack; //Means once player enters attack range, enemy will wait firerate befor firing
    [SerializeField] private int fireRate; //Bigger the number, lower the fire rate
    [SerializeField] private float bulletSpread; //Maximum possible deviation from attempted target

    [Header("Projectile Behavior")]
    [SerializeField] private bool doNotPassUntoBullet;
    //[SerializeField] private bool specialRangedAttack; //When turned on, ... All below "Projectile Behavior" options will be ignored, a spetialized bullet prefab should be used
    [SerializeField] private float bulletLifeSpan;
    [SerializeField] private float bulletVelocity; 
    [SerializeField] private float gravitationalEffect;
    [SerializeField] private HomeingTypes homeingType;
    [SerializeField] private float bulletHomingPower;
    [SerializeField] private BulletFireTypes bulletFireType; //TODO

    [Header("Projectile Collisions")]
    [SerializeField] private bool bulletsDestroyedByObstacles; 
    [SerializeField] private bool projectileFriendlyFire;
    [SerializeField] private bool projectileCollidesWithFriendly;
    [SerializeField] private bool projectileCollidesWithFriendlyBullets;
    [SerializeField] private bool projectileCollidesWithPlayerBullets;

    [Header("Projectile Explotion")]
    [SerializeField] private bool projectileExplodesOnDestruction; //TODO
    [SerializeField] private bool projectileExplosionDamageFallsOff; //TODO //Also applies to force
    [SerializeField] private float projectileExplosionForce; //TODO
    [SerializeField] private float projectileExplosionDamage; //TODO
    [SerializeField] private float projectileExplosionRange; //TODO


    [Header("Attack Range")]
    [SerializeField] private float agroMoveSpeed;
    [SerializeField] private bool useVisualAgroRanges;
    [SerializeField] private float agroRange;
    [SerializeField] private bool useVisualAttackRanges;
    [SerializeField] private float attackRangeOuter;
    [SerializeField] private float attackRangeInner;
    [SerializeField] private bool centerTarget; //Attemtps to keep the player in the center of its attack range

    [Header("Final Blast")] //TODO Everything
    [SerializeField] private bool finalBlast;
    [SerializeField] private bool finalBlastDamageFallsOff; //Also applies to force
    [SerializeField] private float finalBlastForce;
    [SerializeField] private float finalBlastSetoffRange;
    [SerializeField] private float finalBlastDamage;
    [SerializeField] private float finalBlastRadius;

    [Header("Patrol")]
    [SerializeField] private bool useVisualPatrolPoints;
    [SerializeField] private bool beginPatrolRight;
    [SerializeField] private bool resetPatrolRangeWhenDeAgroed; //If true, when de-agroed, patrol points will be reset in relation to current position
    [SerializeField] private float patrolMoveSpeed;
    [SerializeField] private float patrolRangeLeft;
    [SerializeField] private float patrolRangeRight;
    [SerializeField] private float patrolRangeLeftY;
    [SerializeField] private float patrolRangeRightY;

    [Header("Other")]
    [SerializeField] private bool points;

    [Header("Sounds")]

    [Header("Debug")]
    [SerializeField] private bool keepVisualMarkers;

    private void OnValidate()
    {
        health = Mathf.Max(0f, health);
        invincibilityTime = Mathf.Max(0, invincibilityTime);

        projectilesPerFire = Mathf.Max(0, projectilesPerFire);
        projectilesPerBurst = Mathf.Max(0, projectilesPerBurst);
        bulletLifeSpan = Mathf.Max(0f, bulletLifeSpan);
        fireRate = Mathf.Max(0, fireRate);

        finalBlastRadius = Mathf.Max(0f, finalBlastRadius);
        finalBlastSetoffRange = Mathf.Max(0f, finalBlastSetoffRange);

        agroRange = Mathf.Max(0f, agroRange);
        attackRangeOuter = Mathf.Max(0f, attackRangeOuter);
        attackRangeOuter = Mathf.Max(0f, attackRangeInner);

        patrolRangeLeft = Mathf.Max(0f, patrolRangeLeft);
        patrolRangeLeft = Mathf.Max(0f, patrolRangeRight);
    }

    private bool neverAgroed = true;
    private bool justAgroed = false;
    private Vector3 rightPatrolPoint;
    private Vector3 leftPatrolPoint;
    private bool movingToRightPatrol;
    private float middleRangeDistance;
    private int timeToVincibility = 0;
    private int timeTillShoot = 0;
    private int burstRemaining = 0;
    private float timeTillBurstContinue;
    private bool noWeopon = false;

    private GameObject[] players;
    
    void Start()
    {
        AutoConnections();
        CheckForVisualRanges();
        InitializePartol();

        middleRangeDistance = (attackRangeInner + attackRangeOuter)/2f;

        //Attack prep
        burstRemaining = projectilesPerBurst;
        timeTillBurstContinue = timeBetweenBurstFiredProjectiles;

        if (flys) {rigBod.gravityScale = 0;}
        if (ignorAllCollision) {MainCollider.isTrigger = true;}

        if (prefabGun != null)
        {
            gun = Instantiate(prefabGun, new Vector3(transform.position.x, transform.position.y, -2f), transform.rotation);
        }
        gun.transform.parent = gameObject.transform;
        if (gun == null)
        {
            noWeopon = true;
            Debug.Log("No weapon equipped!!!");
        }

    }

    void FixedUpdate()
    {
        GeneralNumbersUpdate();

        WeoponRotationControl();

        ///Movement
        if ((GetDistanceToPlayer() < agroRange) || (!neverAgroed && hyperAgro))
        {
            //Agroed
            int attackCheck;
            neverAgroed = false;
            justAgroed = true;

            if (flys) {attackCheck = AgroFly();} else {attackCheck = AgroMove();}

            if (!chasePlayer)
            {
                rigBod.linearVelocity = new Vector2(0, 0);
                if (flys) {FlyToPatrolPoint();} 
                else {MoveToPatrolPoint();}
            }

            if (attackCheck == 0) {Attack();}
            else  
            {//If charge attack, reset cool down when out of range.
                if (chargeAttack) {timeTillShoot = fireRate;}

                //Continues burst
                if (burstRemaining != projectilesPerBurst)
                {
                    Attack();
                }
            }
        }
        else
        {
            //Not Agroed (Patrol)  
            if (justAgroed)
            {
                justAgroed = false;
                if (resetPatrolRangeWhenDeAgroed) {SetPatrolPoints();}
            }
            if (flys) {if(leftPatrolPoint != transform.position) {FlyToPatrolPoint();} else {rigBod.linearVelocity = new Vector3(0,rigBod.linearVelocity.y,0);}} 
            else {if(leftPatrolPoint.x != transform.position.x) {MoveToPatrolPoint();} else {rigBod.linearVelocity = new Vector3(0,rigBod.linearVelocity.y,0);}}
        }

        AnimateSelf();

    }

    void OnDestroy()
    {
        //Final blast //TODO this is basically in enemy bullet script, make sure to update changes there
        float distanceFromPlayer = GetDistanceToPlayer();
        if (finalBlast && distanceFromPlayer <= finalBlastRadius)
        {
            if (finalBlastDamageFallsOff)
            {
                finalBlastDamage /= distanceFromPlayer*(finalBlastDamage/finalBlastRadius); 
                finalBlastForce /= distanceFromPlayer*(finalBlastForce/finalBlastRadius); 
            }

            //player.TakeDamage(finalBlastDamage)

            //Applies force //TODO test
            Vector3 direction = new Vector3(player.transform.position.x, player.transform.position.y, 0f) - transform.position;
            direction.z = 0f;
            player.GetComponent<Rigidbody2D>().AddForce(direction.normalized * finalBlastForce, ForceMode2D.Impulse);
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
         if (!keepVisualMarkers) {Destroy(patrolPointLeft); Destroy(patrolPointRight);}

        if (useVisualAgroRanges)
        {
            agroRange = Vector3.Distance(transform.position, agroRangeVisual.transform.position);
        }
        if (!keepVisualMarkers) {Destroy(agroRangeVisual);}

        if (useVisualAttackRanges)
        {
            attackRangeOuter = Vector3.Distance(transform.position, outerAttkRangeVisual.transform.position);
            attackRangeInner = Vector3.Distance(transform.position, innerAttkRangeVisual.transform.position);
        }
        if (!keepVisualMarkers) {Destroy(outerAttkRangeVisual); Destroy(innerAttkRangeVisual);}

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
        //if (noWeopon) {return;}
        if (chargeAttack)
        {
            timeTillShoot -= 1;
        }

        if (timeTillShoot <= 0)
        {

            if (burstRemaining > 0)
            {
                if (timeTillBurstContinue <= 0 || burstRemaining == projectilesPerBurst) 
                {
                    for (int i = 0; i < projectilesPerFire; i++)
                    {
                        CreateProjectile();
                    }
                    burstRemaining --;
                    timeTillBurstContinue = timeBetweenBurstFiredProjectiles;
                }
            }
            else
            {
                timeTillShoot = fireRate;
                burstRemaining = projectilesPerBurst;
                timeTillBurstContinue = timeBetweenBurstFiredProjectiles;
            }
            timeTillBurstContinue -= 1;
            
        }
    }

    private void CreateProjectile()
    {
        float spread = bulletSpread/2;
        spread = UnityEngine.Random.Range(-spread, spread);

        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer = directionToPlayer.normalized;
        directionToPlayer.x += spread;
        directionToPlayer.y += spread;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90 + spread;
        angle = Mathf.Repeat(angle, 360);
        angle -= transform.rotation.z;

        GameObject newBullet = Instantiate(PrefabBullet, transform.position, Quaternion.Euler(0, 0, angle));
        BasicEnemyBullet script = newBullet.GetComponent<BasicEnemyBullet>();

        newBullet.GetComponent<Rigidbody2D>().AddForce(directionToPlayer * bulletVelocity, ForceMode2D.Impulse);
        Debug.Log(directionToPlayer);
        if (!doNotPassUntoBullet) //In case the bullet is doing its own special stuff
        {//Sets bullet variables
            script.player = player;
            script.parent = gameObject;

            script.damage = bulletDamage;
            script.lifeSpan = bulletLifeSpan;
            script.velocity = bulletVelocity;

            script.gravitationalEffect = gravitationalEffect;
            script.homingPower = bulletHomingPower;
            script.homeingType = homeingType;

            script.explodesOnDestruction = projectileExplodesOnDestruction;
            script.explosionDamageFallsOff = projectileExplosionDamageFallsOff;
            script.explosionForce = projectileExplosionForce;
            script.explosionDamage = projectileExplosionDamage;
            script.explosionRange = projectileExplosionRange;

            script.destroyedByObstacles = bulletsDestroyedByObstacles;
            script.friendlyFire = projectileFriendlyFire;
            script.collidesWithFriendly = projectileCollidesWithFriendly;
            script.collidesWithFriendlyBullets = projectileCollidesWithFriendlyBullets;
            script.collidesWithPlayerBullets = projectileCollidesWithPlayerBullets;
        }
    }
    
    private float GetDistanceToPlayer()
    {
        if (player != null)
        {
            float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceFromPlayer;
        }
        else
        {
            return 10000;
        }

    }

    private int AgroMove()
    {
        if (IsInAttackRange())
        {
            if (centerTarget) {CenterTarget();}
            rigBod.linearVelocityX = 0;
            return 0; //Attack
        }
        else if (!IsOnGround()) //Don't move if in the air
        {
            return -156;
        }
        else if (chasePlayer)
        {
            if (GetDistanceToPlayer() > attackRangeOuter)
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
            else //if (GetDistanceToPlayer() < attackRangeInner)
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
        }
        return -999;
    }

    private int AgroFly()
    {
        if (IsInAttackRange())
        {
            if (!activeAgroFlight) {rigBod.linearVelocity = new Vector3(0,0,0);}
            return 0; //Attack
        }
        else
        {    
            Vector3 nextPoint = player.transform.position;
            Vector3 direction = nextPoint - transform.position;
            direction = direction.normalized;
            if (GetDistanceToPlayer() > attackRangeOuter) {rigBod.linearVelocity = direction*patrolMoveSpeed;}
            else {rigBod.linearVelocity = -direction*patrolMoveSpeed;}
              
            return -10;
        }
    }

    private void CenterTarget()
    {//TODO
        if (flys)
        {
            
        }
        else
        {
            
        }
    }

    private bool IsInAttackRange()
    {
        if ((GetDistanceToPlayer() < attackRangeOuter) && (GetDistanceToPlayer() > attackRangeInner))
        {
            return true;
        }
        return false;
        

    }
    
    private void FlyToPatrolPoint()
    {
        Debug.Log(transform.position);

        Vector3 nextPoint;
        if (movingToRightPatrol) {nextPoint = rightPatrolPoint;}
        else {nextPoint = leftPatrolPoint;}

        if (Vector3.Distance(transform.position, nextPoint) < 1)
        {
            if (movingToRightPatrol) {movingToRightPatrol = false;}
            else {movingToRightPatrol = true;}
            Debug.Log(nextPoint);
            return;
        }

        Vector3 direction = nextPoint - transform.position;
        direction = direction.normalized;
        rigBod.linearVelocity = direction*patrolMoveSpeed;
    }
    
    private void MoveToPatrolPoint()
    {
        if (!IsOnGround()) {return;}

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
             rightPatrolPoint = new Vector3(transform.position.x + patrolRangeRight, transform.position.y + patrolRangeRightY, 0);
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
    
    private bool IsOnGround()
    {
        float hbSizeX = MainCollider.bounds.size.x;
        float hbSizeY = MainCollider.bounds.size.y;

        Vector2 position = transform.position;
        position.x -= hbSizeX/2f;
        position.y -= (hbSizeY/2f + .1f);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.right, hbSizeX, groundMask);
        if (hit.collider != null) {
            return true;
        }
        return false;
    }

    private bool IsWallAtPosition(string Direction)
    {
        //return false;
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
        if (GetDistanceToPlayer() < agroRange)
        {
            switch (bulletFireType)
            {
                case BulletFireTypes.Standered:
                    Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, 0f);
                    Vector3 direction = playerPos - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                    angle = Mathf.Repeat(angle, 360);
                    angle -= transform.rotation.z;
                    gun.transform.eulerAngles = new Vector3(0, 0, angle+180);
                break;
            }
        }
        else
        {
                gun.transform.eulerAngles = transform.eulerAngles;
        }
    }

    private void AnimateSelf()
    {
        //return;
        if ((GetDistanceToPlayer() < agroRange) || (!neverAgroed && hyperAgro))
        {
            if (transform.position.x - player.transform.position.x > 0) // Look right
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            if (movingToRightPatrol)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
            
        }

        if (timeToVincibility > 0)
        {
            
        }
        
    }

    private void AutoConnections()
    {
        FindPlayer();
        rigBod = GetComponent<Rigidbody2D>();
       
        MainCollider = GetComponent<Collider2D>();
       
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log(spriteRenderer);
    }
    
    private void FindPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int len = players.Length;
        if (len > 1)
        {
            float closestDistance = -100;
            for(int i = 0; i < len; i++)
            {
                GameObject obj = players[i];
                float dist = Vector3.Distance(obj.transform.position, transform.position);
                if (dist < closestDistance || closestDistance == -100)
                {
                    closestDistance = dist;
                    player = obj;
                }
            }
        }
        else if (len == 1)
        {
            player = players[0];
        }
        else
        {
            Debug.Log("No player found");
        }
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
