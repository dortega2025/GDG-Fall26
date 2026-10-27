using System.Collections;
using System.Linq;
using Unity.Tutorials.Editor;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;
    private GameObject player;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject laserLeft;
    [SerializeField] private GameObject laserRight;
    private float turretSpeed = 5f;
    private float laserSpeed = 10f;
    private int currMove;
    private float timeSinceShot;
    private float timeSinceLaser;
    private float shotTimer = 2f;
    private int shotAmount = 3;
    private float laserTimer = 2f;
    private int phaseNum;
    private bool phaseChangeLeft;
    private bool phaseChangeRight;
    private bool phaseChange3;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject laserPrefabL;
    [SerializeField] private GameObject laserPrefabR;
    private float fireSpeed = 10f;
    [SerializeField] private GameObject[] path;
    [SerializeField] private GameObject[] laserPathL;
    private int pathIdxL = 0;
    [SerializeField] private GameObject[] laserPathR;
    private int pathIdxR = 0;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        timeSinceShot = 0f;
        currMove = 1;
        phaseNum = 2;
        phaseChangeLeft = false;
        phaseChangeRight = false;
        timeSinceLaser = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceShot += Time.deltaTime;
        if (timeSinceShot > shotTimer)
        {
            StartCoroutine(Shoot());
            timeSinceShot = 0;
        }
        Aim();
        TurretMove();
        if (phaseNum == 2 && phaseChangeLeft == false && phaseChangeRight == false)
        {
            LaserMove(laserLeft, laserPathL, ref pathIdxL);
            LaserMove(laserRight, laserPathR, ref pathIdxR);
        }
        if (phaseNum >= 2 && phaseChangeLeft == true)
        {
            timeSinceLaser += Time.deltaTime;
            if (timeSinceLaser > laserTimer)
            {
                Laser();
                laserTimer = Random.Range(20f, 30f);
                timeSinceLaser = 0;
            }
            
        }
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < shotAmount; i++)
        {
            GameObject bullet;
            Vector3 direction = player.transform.position - barrel.transform.position;
            direction.z = 0f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            angle = Mathf.Repeat(angle, 360);
            direction = direction.normalized;
            bullet = Instantiate(bulletPrefab, barrel.transform.position, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<Rigidbody2D>().AddForce(direction * fireSpeed, ForceMode2D.Impulse); 
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Laser()
    {
        GameObject laserL;
        GameObject laserR;
        laserL = Instantiate(laserPrefabL, laserLeft.transform.GetChild(0).transform.position, Quaternion.identity);
        laserR = Instantiate(laserPrefabR, laserRight.transform.GetChild(0).transform.position, Quaternion.identity);
    }

    void Aim()
    {
        Vector3 direction = player.transform.position - barrel.transform.position;
        direction.z = 0f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        angle = Mathf.Repeat(angle, 360);
        angle -= transform.rotation.z;
        turret.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    void TurretMove()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        angle = Mathf.Repeat(angle, 360);
        if (angle > 200f && currMove == 1)
        {
            currMove = 2;
        } else if (angle < 200f && currMove == 2)
        {
            currMove = 1;
        } else if (angle < 160f && currMove == 1)
        {
            currMove = 0;
        } else if (angle > 160f && angle < 200f && currMove == 0)
        {
            currMove = 1;
        } else if (angle > 160f && angle < 200f && currMove == 2)
        {
            currMove = 1;
        }
        turret.transform.position = Vector2.MoveTowards(turret.transform.position, path[currMove].transform.position, turretSpeed * Time.deltaTime);
    }

    void LaserMove(GameObject laser, GameObject[] path, ref int pathIdx)
    {
        if (laser.transform.position != path[pathIdx].transform.position)
        {
            Vector3 dir = path[pathIdx].transform.position - laser.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            angle = Mathf.Repeat(angle, 360);
            angle -= laser.transform.rotation.z;
            laser.transform.eulerAngles = new Vector3(0, 0, angle);
            laser.transform.position = Vector2.MoveTowards(laser.transform.position, path[pathIdx].transform.position, laserSpeed * Time.deltaTime);
        } else
        {
            pathIdx++;
        }
        if (laser == laserLeft && pathIdx >= path.Length)
        {
            laser.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            phaseChangeLeft = true;
        } else if (laser == laserRight && pathIdx >= path.Length)
        {
            laser.transform.eulerAngles = new Vector3(0f, 0f, 270f);
            phaseChangeRight = true;
        }
    }
}
