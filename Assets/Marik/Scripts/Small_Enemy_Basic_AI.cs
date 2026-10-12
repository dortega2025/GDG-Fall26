using System;
using NUnit.Framework;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Small_Enemy_Basic_AI : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody2D rigBod;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject gun;
    [SerializeField] LayerMask groundMask;

    [Header("Behavior")]
    [SerializeField] bool hyperAgro; //If true, one agroed, won't de-agro
    //[SerializeField] bool canJump;
    [SerializeField] bool isSmart; //If true, won't fall in pits

    [Header("Attack")]
    [SerializeField] bool bulletsDestroyedByObstacles;
    [SerializeField] float fireRate;
    [SerializeField] float bulletVelocity;
    [SerializeField] float bulletSpread;

    [Header("General Stats")]
    [SerializeField] float agroMoveSpeed;
    [SerializeField] float health;

    [Header("Range")]
    [SerializeField] float agroRange;
    [SerializeField] float attackRangeOuter;
    [SerializeField] float attackRangeInner;

    [Header("Patrol")]
    [SerializeField] bool resetRangeWhenDeAgroed; //If true, when de-agroed, patrol points will be reset in relation to current position
    [SerializeField] float patrolMoveSpeed;
    [SerializeField] float patrolRangeLeft;
    [SerializeField] float patrolRangeRight;

    private bool neverAgroed = true;

    float GetDistance()
    {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceFromPlayer;
    }
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        ///Movement
        if ((GetDistance() < agroRange) || (!neverAgroed && hyperAgro))
        {
            //Agroed
            neverAgroed = false;
            int attackCheck = AgroMove();
            if (attackCheck == 0)
            {
                //Attack
            }
        }
        else
        {
            //Not Agroed (Patrol)
            
        }
    }

    private int AgroMove()
    {
        if ((GetDistance() < attackRangeOuter) && (GetDistance() > attackRangeInner))
        {
            rigBod.linearVelocityX = 0;
            return 0; //Attack
        }
        else if (GetDistance() > attackRangeOuter)
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
}
