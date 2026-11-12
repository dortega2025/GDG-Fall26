using Unity.VisualScripting;
using UnityEngine;

public class EndlessRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    void Start()
    {
        
    }


    void FixedUpdate()
    {
        Vector3 temp = transform.eulerAngles;
        temp.z += rotationSpeed;
        transform.eulerAngles = temp;
    }
}
