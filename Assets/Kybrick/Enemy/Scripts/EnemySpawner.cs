using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeReference] private GameObject Enemy;
    void Start()
    {
        Instantiate(Enemy, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
