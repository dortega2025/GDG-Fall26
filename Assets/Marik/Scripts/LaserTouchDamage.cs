using UnityEngine;

public class LaserTouchDamage : MonoBehaviour
{
    private GameObject player;
    [SerializeField] float damage;

    private GameObject healthLogic;
    private Health health;

    void Start()
    {
        FindPlayer();

        healthLogic = GameObject.Find("HealthLogic");
        health = healthLogic.GetComponent<Health>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if (Collider2D.gameObject.tag == "Player")
        {
            health.TakeDamage(damage);
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
