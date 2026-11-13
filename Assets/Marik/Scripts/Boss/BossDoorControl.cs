using Unity.VisualScripting;
using UnityEngine;

public class BossDoorControl : MonoBehaviour
{
    [SerializeField] public GameObject relatedBoss;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (relatedBoss == null)
        {
            Destroy(gameObject);
        }
    }
}
