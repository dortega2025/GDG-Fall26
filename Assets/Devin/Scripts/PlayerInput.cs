using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Cinemachine.SplineAutoDolly;

public class PlayerInput : MonoBehaviour
{
    [Header("ShootSettings")]
    [SerializeField] private float AtkSpeed;
    [SerializeField] private float AtkTimer;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private float Atk;
    [SerializeReference] private GameObject bulletprefab;
    [SerializeReference] private GameObject Generator;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCount = 0;
    [SerializeField] private float jumpMAX = 2;

    [Header("Dash Settings")]
    [SerializeField] private float dashPower = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private GameObject Squish;
    [SerializeField] private float SquishAmount;
    [SerializeField] private float SquishMax;
    private float SquishScaleX;
    private float SquishScaleY;


    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Sprite Handling")]
    private bool IsFacingRight = true;

    [Header("Health and Damage and Stuff")]
    [SerializeField] private Health health; //temp to handle damage
    


    private Rigidbody2D rb;
    private Vector2 moveInput;

    
    // Dash state tracking
    private bool canDash = true;
    private bool isDashing;
    private float lastFacingDirection = 1f; 

    void Start()
    {
        SquishScaleX = Squish.transform.localScale.x;
        SquishScaleY = Squish.transform.localScale.y;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        jumpCount = jumpMAX;
        GlobalAmmo.handgunAmmoCount = GlobalAmmo.handgunAmmoCountMax;
    }

    // Handles WASD / Left Stick Left and Right
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // Track the last direction the player moved so they can dash 
        // even if they standing still when they press the dash button.
        if (moveInput.x != 0)
        {
            lastFacingDirection = Mathf.Sign(moveInput.x);
        }
    }

    

    // Handles the Space Key / Jump Button Action
    public void Jump(InputAction.CallbackContext context)
    {
         // 1. ONLY trigger when the button is first pressed down
        if (!context.started) return;

        // 2. Prevent jumping while actively dashing
        if (isDashing) return;

        // 3. Check if the player has available jumps left
        if (IsGrounded() || jumpCount > 0)
        {
            // If they are mid-air and jumping for the first time, consume an extra jump charge
            if (!IsGrounded() && jumpCount == jumpMAX)
            {
                Debug.Log("Extra Jump Consumed");
                jumpCount--; 
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
            Debug.Log("Jump Consumed");
        }
    
    }
    

    // NEW: Handles the Dash Button Action
    public void Dash(InputAction.CallbackContext context)
    {
        // Trigger the dash only when the button is first pressed down
        if (context.performed) Debug.Log("Dash Button Pressed"); 
        if (context.performed && canDash)
        {
            StartCoroutine(PerformDash());
            Debug.Log("PerformDashTriggered");
        }
    }

    //Temp code that uses the player input key 'z' to take damage. Eventually this will need to get replaced.
    public void TempTakeDamage(InputAction.CallbackContext context)
    {
        if (AtkTimer >= 0 && AtkTimer - 1*Time.deltaTime >=0)
        {
            AtkTimer -= 1 * Time.deltaTime;
        }
        else
        {
            AtkTimer = 0;
        }
        //Squish
        if (isDashing && Squish.transform.localScale.y <= 1)
        {
            if (Squish.transform.localScale.y - SquishAmount >= SquishMax)
            {
                SquishScaleY -= SquishAmount;
                Squish.transform.localScale = new Vector3(1, SquishScaleY, 0);
            }
        }
        else if (isDashing == false)
        {
            
                if (Squish.transform.localScale.y + SquishAmount <= 1)
                {
                    SquishScaleY += SquishAmount;
                    Squish.transform.localScale = new Vector3(1, SquishScaleY, 0);
                }
                else
                {
                 SquishScaleY = 1;
                 Squish.transform.localScale = new Vector3(1, SquishScaleY, 0);
                }

        }
            if (knockbackTimer > 0)
        {   
        knockbackTimer -= Time.fixedDeltaTime;
        return; // Exits FixedUpdate early so keys can't override the force
        }
        
    }

    void FixedUpdate()
    {
        
        // Stop normal movement physics if the player is currently dashing
        if (isDashing) return;

        // Normal movement
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    
        if (IsGrounded())
        {
            jumpCount = jumpMAX;
        }

            turnCheck();

    }

    // Coroutine to handle the dash burst safely outside FixedUpdate
    private IEnumerator PerformDash()
    {
         Debug.Log("PerformDash Executed");
        canDash = false;
        isDashing = true;
      
        // Remember the regular gravity setting so we can restore it later
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // Turn off gravity so the player doesn't dip downwards mid-dash

        // Determine dash direction (uses current input, falls back to last faced direction)
        float dashDirection = moveInput.x != 0 ? Mathf.Sign(moveInput.x) : lastFacingDirection;

        // Apply sudden burst velocity horizontally, locking vertical movement entirely
        rb.linearVelocity = new Vector2(dashDirection * dashPower, 0f);

        // Wait out the duration of the dash
        yield return new WaitForSeconds(dashTime);

        // Restore normal physics states
        rb.gravityScale = originalGravity;
        isDashing = false;

        // Enforce the cooldown before letting them dash again
        yield return new WaitForSeconds(dashCooldown);
        Debug.Log("Dash Reset (Cooldown)");
        canDash = true;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    

    private void turnCheck()
    {
        if (lastFacingDirection > 0 && !IsFacingRight )
        {
            Turn();
        }
        else if (lastFacingDirection < 0 && IsFacingRight )
        {
            Turn();
        }
    }

    private void Turn()
    {
        Debug.Log("Turned");
        if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
         else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }
    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && AtkTimer == 0)//&& CanShoot
        {
            AtkTimer = AtkSpeed;
            Shoot();
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        angle = Mathf.Repeat(angle, 360);
        angle = angle - transform.rotation.z;
        direction.z= 0;
        direction = direction.normalized;
        bullet = Instantiate(bulletprefab, Generator.transform.position, Quaternion.Euler(0, 0, angle));
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * BulletSpeed, ForceMode2D.Impulse);
    }
}