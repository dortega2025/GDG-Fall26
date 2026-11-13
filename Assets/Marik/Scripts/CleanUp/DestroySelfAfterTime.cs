using Unity.VisualScripting;
using UnityEngine;

public class DestroySelfAfterTime : MonoBehaviour
{

    [SerializeField] int timeTillDestory;

    void FixedUpdate()
    {
        timeTillDestory--;
        if (timeTillDestory < 0)
        {
            Destroy(gameObject);
        }
    }
}
