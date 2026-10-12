using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float jumpForce = 20f;
    public Rigidbody2D rb;
    private Vector2 inputVector = Vector2.zero;
    public float checkRadius = 0.1f;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded;
    public int HP;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject edge;
    [SerializeField] private GameObject arm;
    private float fireSpeed = 10f;
    private float timeSinceShot;
    private float shotTimer = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeSinceShot = shotTimer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        Movement();
        timeSinceShot += Time.deltaTime;
    }

    bool CheckGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1f;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundMask);
        if (hit.collider != null) {
            return true;
        }
        return false;
    }
  
    void Movement()
    {
        rb.linearVelocityX = inputVector.x * moveSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.linearVelocityY = jumpForce;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && timeSinceShot > shotTimer)
        {
            Shoot();
            timeSinceShot = 0;
        }
    }

    void Shoot()
    {
        GameObject bullet;
        float mousePosX = Mouse.current.position.x.ReadValue();
        float mousePosY = Mouse.current.position.y.ReadValue();
        Vector3 mousePos = new Vector3(mousePosX, mousePosY, 0);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 direction = mousePos - transform.position;
        direction.z = 0f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        angle = Mathf.Repeat(angle, 360);
        angle -= transform.rotation.z;
        direction = direction.normalized;
        bullet = Instantiate(bulletPrefab, edge.transform.position, Quaternion.Euler(0, 0, angle));
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * fireSpeed, ForceMode2D.Impulse);
    }
}
