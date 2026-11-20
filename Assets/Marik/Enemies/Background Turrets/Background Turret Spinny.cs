using System;
using UnityEngine;
using Unity.Mathematics;

public class BackgroundTurretSpin : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private GameObject turretObj;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private GameObject shootSound;

    [Header("Stats")]
    [SerializeField] private float range;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float attkSpeed;

    [Header("Bullet Stats")]
    [SerializeField] public float damage;
    [SerializeField] public float velocity;

    private float attackMaxTime;
    private GameObject player;

    void Start()
    {
        attackMaxTime = attkSpeed;
        FindPlayer();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < range)
        {
            turretObj.transform.eulerAngles += new Vector3(0,0, rotateSpeed);
            if (attkSpeed >= 0) {attkSpeed--;}
            else
            {
                Instantiate(shootSound);
                attkSpeed = attackMaxTime;
                GameObject newProjectile = Instantiate(bullet, bulletSpawnPoint.transform.position, turretObj.transform.rotation * quaternion.Euler(0, 0, 90));
                Vector3 r1 = turretObj.transform.rotation * Vector3.left;
                Vector2 r2 = new Vector2(r1.x, r1.y).normalized*velocity;
                newProjectile.GetComponent<Rigidbody2D>().AddForce(r2, ForceMode2D.Impulse);
                newProjectile.GetComponent<BasicEnemyBullet>().damage = damage;
            }
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
