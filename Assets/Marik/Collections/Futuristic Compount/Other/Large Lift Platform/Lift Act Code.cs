using System;
using UnityEngine;

public class LiftActCode : MonoBehaviour
{
    [SerializeField] GameObject liftToActivate;
    [SerializeField] bool moving;
    [SerializeField] Rigidbody2D rigBod;

    [Header("Gen Options")]
    [SerializeField] GameObject finalPosObj;
    [SerializeField] float speed;

    [Header("Astetic")]
    [SerializeField] GameObject[] rails;
    [SerializeField] Sprite activeRailsTexture;
    [SerializeField] GameObject startSound;
    [SerializeField] GameObject endSound;

    [Header("Debug")]
    [SerializeField] float distanceFromEnd;

    private GameObject player;
    private SpriteRenderer[] sRenders;
    private Sprite ogSprite;

    private Vector3 finalPos;

    void Start()
    {
        sRenders = new SpriteRenderer[rails.Length];
        FindPlayer();
        finalPos = finalPosObj.transform.position;
        Destroy(finalPosObj);
        ogSprite = rails[0].GetComponent<SpriteRenderer>().sprite;
        for(int i = 0; i < rails.Length; i++)
        {
            sRenders[i] = rails[i].GetComponent<SpriteRenderer>();
        }
        
    }


    void FixedUpdate()
    {
        distanceFromEnd = Vector3.Distance(finalPos, liftToActivate.transform.position);
        if (moving)
        {

            Move();
            AdjustPlayer();
            if (Vector3.Distance(finalPos, liftToActivate.transform.position) <= 10 && speed > .6)
            {
                speed /= 1.01f;
            }
            if (Vector3.Distance(finalPos, liftToActivate.transform.position) <= 2.5)
            {
                if (endSound != null) {Instantiate(endSound);}
                rigBod.linearVelocity = new Vector2(0,0);
                //liftToActivate.transform.position = finalPos;
                moving = false;
                for(int i = 0; i < rails.Length; i++)
                {
                    sRenders[i].sprite = ogSprite;
                }
            }
        }

    }

    private void AdjustPlayer()
    {
        Rigidbody2D pRigBode = player.GetComponent<Rigidbody2D>();
        pRigBode.linearVelocity += rigBod.linearVelocity - new Vector2(0, rigBod.linearVelocity.y*1f);
    }

    private void Move()
    {
        Vector3 direction = (finalPos - transform.position).normalized;
        rigBod.linearVelocity = direction*speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (startSound != null) {Instantiate(startSound);}
            moving = true;
            for(int i = 0; i < rails.Length; i++)
            {
                sRenders[i].sprite = activeRailsTexture;
            }
            Destroy(gameObject.GetComponent<Collider2D>());
        }
    }

    private void FindPlayer()
    {
        GameObject[] players;
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
}
