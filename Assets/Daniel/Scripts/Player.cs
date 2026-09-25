using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float jumpForce = 10f;
    public Rigidbody2D rb;
    private Vector2 inputVector = Vector2.zero;
    public float checkRadius = 0.1f;
    public LayerMask groundMask;
    private bool isGrounded;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject edge;
    [SerializeField] private GameObject arm;
    private float fireSpeed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        Movement();
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
        if (context.performed)
        {
            Shoot();
        }
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }

    void Shoot()
    {
        GameObject bullet;
        float mousePosX = Mouse.current.position.x.ReadValue();
        float mousePosY = Mouse.current.position.y.ReadValue();
        Vector3 mousePos = new Vector3(mousePosX, mousePosY, 0f);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        angle = Mathf.Repeat(angle, 360);
        angle = angle - transform.rotation.z;
        bullet = Instantiate(bulletPrefab, edge.transform.position, Quaternion.Euler(0, 0, angle));
        bullet.GetComponent<Rigidbody2D>().AddForce(direction.normalized * fireSpeed, ForceMode2D.Impulse);
    }
}
