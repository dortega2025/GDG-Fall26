using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrGunBehavior : MonoBehaviour
{

    [SerializeField] GameObject Bullet;


    private Vector3 mousePos;
    private float angle;
    public void Fire(InputAction.CallbackContext context)
    {
        Instantiate(Bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
         mousePos = Input.mousePosition;
         Debug.Log(mousePos);
         angle = math.atan((mousePos.y - transform.position.y)/(mousePos.x - transform.position.x));
         angle *= MathF.PI - (180f/3.141592653f);
         Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
         transform.rotation = rotation;
    }
}
