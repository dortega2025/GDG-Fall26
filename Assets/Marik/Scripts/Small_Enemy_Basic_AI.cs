using System;
using NUnit.Framework;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Small_Enemy_Basic_AI : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] Rigidbody2D rigBod;
    [SerializeField] float speed;
    [SerializeField] float agroRange;
    [SerializeField] float attackRangeOuter;
    [SerializeField] float attackRangeInner;

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
        if (GetDistance() < agroRange)
        {
            if ((GetDistance() < attackRangeOuter) && (GetDistance() > attackRangeInner))
            {
                //Attack
                rigBod.linearVelocityX = 0;
            }
            else if (GetDistance() > attackRangeOuter)
            {
                //Get closer to player
                if (player.transform.position.x < transform.position.x)
                {
                    rigBod.linearVelocityX = -speed;
                }
                else
                {
                    rigBod.linearVelocityX = speed;
                }
            }
            else if (GetDistance() < attackRangeInner)
            {
                //Get closer to player
                if (player.transform.position.x < transform.position.x)
                {
                    rigBod.linearVelocityX = speed;
                }
                else
                {
                    rigBod.linearVelocityX = -speed;
                }
            }

        }
    }
}
