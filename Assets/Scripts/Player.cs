using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 inputVector = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();

    }

    void Movement()
    {
        Vector3 movement = new Vector3(inputVector.x, 0f, 0f);
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, movement.y * moveSpeed, 0f);
    }
}
