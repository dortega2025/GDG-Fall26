using UnityEngine;
using UnityEngine.InputSystem;

public class Scr_PlayerMovement : MonoBehaviour
{
    [Header("Player Component")]
    [SerializeField] Rigidbody2D rigBod;

    [Header("Adjustments")]
    [SerializeField] float jumpStrength;
    [SerializeField] float jumpContinuesStrength;
    [SerializeField] float speed;


    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    private float horizontal; 
    private bool jumpReleased;
    private InputAction actionJump;

    private void FixedUpdate()
    {
    if  (actionJump.IsPressed() && !jumpReleased){
            rigBod.linearVelocityY += jumpContinuesStrength;
        }
        else
        {
            jumpReleased = true;
        }
        rigBod.linearVelocity = new Vector2(horizontal*speed, rigBod.linearVelocity.y);
    }

    public void move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGounded())
        {
            jumpReleased = false;
            rigBod.linearVelocity = new Vector2(rigBod.linearVelocity.x, jumpStrength);
        }
    }

    private bool IsGounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, .1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actionJump = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
