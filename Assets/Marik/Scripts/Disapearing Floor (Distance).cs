using Unity.Mathematics;
using UnityEngine;

public class DisapearingFloor_Distance : MonoBehaviour
{
    [SerializeField] private bool useHorDist;
    [SerializeField] private bool useVertDist;
    [SerializeField] private float horizontalDistance;
    [SerializeField] private float verticalDistance;

    private GameObject[] players;

    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindPlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool xGood = false;
        if (useHorDist)
        {
            if (math.abs(transform.position.x - player.transform.position.x) < horizontalDistance)
            {
                xGood = true;
            }
        }
        else
        {
            xGood = true;
        }

        bool yGood = false;
        if (useVertDist)
        {
            if (math.abs(transform.position.x - player.transform.position.x) < horizontalDistance)
            {
                yGood = true;
            }
        }
        else
        {
            yGood = true;
        }

        if (xGood && yGood)
        {
            Destroy(gameObject);
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