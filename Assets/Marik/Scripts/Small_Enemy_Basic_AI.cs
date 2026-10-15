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
    [Header("Connections")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rigBod;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject gun;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject groundCheck;

    [Header("General Behavior")]
    [SerializeField] private bool hyperAgro; //If true, one agroed, won't de-agro
    //[SerializeField] bool canJump;
    [SerializeField] private bool isSmart; //If true, won't fall in pits
    [SerializeField] private bool flys; //TODO Everything
    [SerializeField] private bool ignorCollision; //TODO 
    [SerializeField] private bool solidToPlayer; //TODO
    [SerializeField] private bool lockDamage; //Forces each hit to this enemy deal one damage
    [SerializeField] private bool chasePlayer;

    [Header("General Stats")]
    [SerializeField] private float health;
    [SerializeField] private float invincibilityTime; //TODO 

    [Header("Attack")] //TODO Everything
    [SerializeField] private float touchDamage;
    [SerializeField] private bool bulletsDestroyedByObstacles;
    [SerializeField] private bool chargeAttack;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float bulletSpread;

    [Header("Attack Range")]
    [SerializeField] private float agroMoveSpeed;
    [SerializeField] private float agroRange;
    [SerializeField] private float attackRangeOuter;
    [SerializeField] private float attackRangeInner;

    [Header("Final Blast")] //TODO Everything
    [SerializeField] private bool finalBlast;
    [SerializeField] private bool finalBlastDamage;
    [SerializeField] private bool finalBlastRadius;
    [SerializeField] private bool finalBlastForce;

    [Header("Patrol")]
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
    private Vector3 prevPosition = new Vector3(0,0,0);
    private float timeToVincibility = 0;
    
    void Start()
    {
        if (flys)
        {
            rigBod.gravityScale = 0;
        }

        SetPatrolPoints();
        if (beginPatrolRight)
        {
            movingToRightPatrol = true;
        }

        if (!solidToPlayer)
        {
            
        }
    }

    void FixedUpdate()
    {
        ///Movement
        if ((GetDistance() < agroRange) || (!neverAgroed && hyperAgro))
        {
            //Agroed
            int attackCheck = -999;
            neverAgroed = false;
            justAgroed = true;

            if (!flys)
            {
                attackCheck = AgroMove();
            }
            else
            {
                attackCheck = AgroFly();
            }

            if (attackCheck == 0)
            {
                //Attack
            }
        }
        else
        {
            //Not Agroed (Patrol)  
            if (justAgroed)
            {
                justAgroed = false;
                SetPatrolPoints();
            }

            if (!flys)
            {
                MoveToPatrolPoint();
            }
            else
            {
                FlyToPatrolPoint();
            }
        }
        prevPosition = transform.position;
    }

    float GetDistance()
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
                    rigBod.linearVelocityX = -agroMoveSpeed;
                }
                else
                {
                    rigBod.linearVelocityX = agroMoveSpeed;
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
                transform.LookAt(rightPatrolPoint);
                
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
                Debug.Log("Patrol changed to left");
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
                Debug.Log("Patrol changed to right");
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

    public void TakeDamage(float DMG)
    {
        if (lockDamage)
        {
            health--;
        }
        else
        {
            health -= DMG;
        }
    }
}