using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ReadyBossParts : MonoBehaviour
{
    [Header("Boss Spawn")]
    [SerializeField] GameObject[] objToSpawns;//Num 1 should be main boss
    [SerializeField] Vector3 relitiveSpawnLocation;
    [SerializeField] GameObject relatedDoor;

    //[Header("Boss Spawn")]
    
    private GameObject[] placeHolderDoors;
    private GameObject closestBossDoor;

    void Start()
    {
        placeHolderDoors = GameObject.FindGameObjectsWithTag("Boss Door Placeholder");
        if (placeHolderDoors.Length < 1) {Debug.Log("Boss door not found"); return;}
        closestBossDoor = placeHolderDoors[0];
        float minDist = Vector3.Distance(placeHolderDoors[0].transform.position, transform.position);
        for(int i = 1; i < placeHolderDoors.Length; i++)
        {
            float dist = Vector3.Distance(placeHolderDoors[i].transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestBossDoor = placeHolderDoors[i];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if (Collider2D.gameObject.CompareTag("Player"))
        {
            GameObject mainboss = null;
            for (int i = 0; i < objToSpawns.Length; i++)
            {
                if (i == 0)
                {
                    mainboss = Instantiate(objToSpawns[i], relitiveSpawnLocation + transform.position, transform.rotation);
                }
                else
                {
                    Instantiate(objToSpawns[i], relitiveSpawnLocation + transform.position, transform.rotation);
                }
            }
            Vector3 pos = closestBossDoor.transform.position;
            Destroy(closestBossDoor);
            GameObject door = Instantiate(relatedDoor, pos, quaternion.Euler(0, 0, 0));
            door.GetComponent<BossDoorControl>().relatedBoss = mainboss;
            Destroy(gameObject);
        }
    }
}
