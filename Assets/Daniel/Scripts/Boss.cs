using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;
    private GameObject player;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject turret;
    private float turretSpeed = 5f;
    private int currMove;
    private float timeSinceShot;
    private float shotTimer = 2f;
    private int shotAmount = 3;
    [SerializeField] private GameObject bulletPrefab;
    private float fireSpeed = 10f;
    [SerializeField] private GameObject[] path;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        timeSinceShot = 0;
        currMove = 1;
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
            //angle -= transform.rotation.z;
            direction = direction.normalized;
            bullet = Instantiate(bulletPrefab, barrel.transform.position, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<Rigidbody2D>().AddForce(direction * fireSpeed, ForceMode2D.Impulse); 
            yield return new WaitForSeconds(0.1f);
        }
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
}
