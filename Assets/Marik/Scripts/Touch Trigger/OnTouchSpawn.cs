using UnityEngine;

public class OnTouchSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] objToSpawns;
    [SerializeField] Vector3 relitiveSpawnLocation;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if (Collider2D.gameObject.tag == "Player")
        {
            for (int i = 0; i < objToSpawns.Length; i++)
            {
                Instantiate(objToSpawns[i], relitiveSpawnLocation + transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}