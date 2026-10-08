using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float moveSpeed = 5f;
    [SerializeField]private float jumpForce;
    [SerializeField] private int JumpNumMax;
    public int JumpNum;
    public Rigidbody2D rb;
    private Vector2 inputVector = Vector2.zero;
    public float checkRadius = 0.1f;
    public LayerMask groundMask;
    private bool isGrounded;
    public int HP;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject edge;
    [SerializeField] private GameObject arm;
    private float fireSpeed = 10f;
    [SerializeField] private float AtkSpeed;//how quickly the player can attack
    private float AtkTimer;
    [SerializeField] private float Invul;
    [SerializeReference] private GameObject MainCam;
    private float InvulTimer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EBullet") && (InvulTimer == 0)) //check if invulnerable
        {
            InvulTimer = Invul;//Set timer
            HP -= 1;//reduce hp, need to change this out to find away to get the damage value from the bullet itself
            Destroy(collision.gameObject);// destroy bullet to stop it from gunking anything up
            if (HP <= 0)//Check here if dead, so we dont wait until no longer invul
            {
                MainCam.transform.parent = null;
                gameObject.SetActive(false);
                ActivateDeathUI.instance.ActivateUI();
            }
        }
        else if (collision.gameObject.CompareTag("EBullet"))
        {
            Destroy(collision.gameObject);//destroys bullet regardless
        }
    }

    void Awake()
    {
        MainCam.transform.parent = transform;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        if (isGrounded)
        {
            JumpNum = JumpNumMax;
        }
        Movement();
    }
    void Update()
    {
        if(InvulTimer - 1*Time.deltaTime >= 0)
        {
            InvulTimer -= 1 * Time.deltaTime;
        }
        else
        {
            InvulTimer = 0;
        }
        if(AtkTimer - 1*Time.deltaTime >= 0)
        {
            AtkTimer -= 1 * Time.deltaTime;
        }
        else
        {
            AtkTimer = 0;
        }
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
        if (context.performed && isGrounded || JumpNum >= 1)
        {
            JumpNum -= 1;
            Jump();
        }
    }

    void Jump()
    {
        JumpNum -= 1;
        rb.linearVelocityY = jumpForce;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && AtkTimer==0)
        {
            AtkTimer = AtkSpeed;
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

    public void TakeDamage(int Dmg)
    {
        if(InvulTimer == 0)
        {
            HP -= Dmg;
            InvulTimer = Invul;
        } 
    }
}
