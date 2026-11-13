using UnityEngine;

public class PlayerDropRestraintLogic : MonoBehaviour
{
    //All relative to the object
    [SerializeField] private bool useHeighestY;
    [SerializeField] private float heighestY;
    [SerializeField] private bool useLowestY;
    [SerializeField] private float lowestY;
    [SerializeField] private float maxSpeed;

    private Rigidbody2D rigBod;

    private GameObject[] players;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindPlayer();
        rigBod = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((player.transform.position.y < (lowestY - transform.position.y) && useLowestY) || ((player.transform.position.y > (lowestY - transform.position.y)) && useHeighestY))
        {
            rigBod.linearDamping = 0;
            Debug.Log("Player max speed reset to none.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if (Collider2D.gameObject.tag == "Player")
        {
            rigBod.linearDamping = .5f;
            Debug.Log("Player max speed slowed.");
        }
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
}
