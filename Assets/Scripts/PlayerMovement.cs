using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement _instance;
    private Rigidbody2D rb;
    public Rigidbody2D GetRB()
    {
        return rb;
    }
    private Vector2 inputThisFrame;
    private readonly Vector3 baseSize = new Vector3(1f,1,1); 

    public LayerMask ground;
    public float height = 2f;

    [Header("Ground Movement")]
    public float walkSpeed = 5f;
    //public float sprintSpeed = 10f;
    public float crouchSpeed = 1f;
    public float normalDrag = 5f;
    private float currentMoveSpeed = 0f;

    [Header("Jump Settings")]
    public float jumpForce = 3f;
    public float jumpDrag = 8f;
    public float maxGravity = 10f;
    public int maxAmountOfJumps = 2;
    private int currentAmountOfJumps;

    [Header("Dash Settings")]
    public float timeBetweenDashes = 0.5f;
    private float timeTillNextDash;

    [Header("Animation")]
    public Transform PlayerSprite;
    private SpriteRenderer sr;

    bool grounded(){
        return Physics2D.Raycast(transform.position, Vector2.down, height/2, ground);
    }
    private bool groundedLastFrame = true;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        _instance = this;
        sr = PlayerSprite.GetComponent<SpriteRenderer>();
    }

    private void Update() {

        inputThisFrame = InputManager.MovementInput();
        SpeedControl();
        StateControl();

        if(inputThisFrame.x != 0)
        {
            sr.flipX = inputThisFrame.x < 0;
        }

        if(InputManager.DashKeyPressed() && timeTillNextDash <= 0)
        {
            Dash(inputThisFrame);
            timeTillNextDash = timeBetweenDashes;
        }
        else
        {
            timeTillNextDash -= Time.deltaTime;
        }

        if(InputManager.JumpButtonPressed() && currentAmountOfJumps >= 1){
            Jump();
        }

        if(grounded()){
            currentAmountOfJumps = maxAmountOfJumps;
        }
        else{
            transform.localScale = baseSize;
        }

        PlayerSprite.localScale = Vector3.MoveTowards(PlayerSprite.localScale, Vector3.one, Time.deltaTime);
        PlayerSprite.localPosition = new Vector3(0,-(1f - PlayerSprite.localScale.y)/2);
    }

    private void FixedUpdate() {
        StandardMove();
    }

    //* Movement Functions
    /// <summary>
    /// Normal Ground movement
    /// </summary>
    private void StandardMove(){
        rb.AddForce(inputThisFrame.x * Vector2.right * currentMoveSpeed, ForceMode2D.Force);
    }

    /// <summary>
    /// Jump 
    /// </summary>
    private void Jump(){
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        PlayerSprite.localScale = new Vector3(0.7f,1.1f,1f);
        rb.gravityScale *= 0.5f;
        currentAmountOfJumps--;
    }

    /// <summary>
    /// Dash 
    /// </summary>
    private void Dash(Vector2 inputThisFrame)
    {
        StartCoroutine(nameof(DashCo),inputThisFrame);
    }

    IEnumerator DashCo(Vector2 inputThisFrame)
    {
        rb.AddForce(1.2f * Vector2.up, ForceMode2D.Impulse);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        rb.AddForce(3.0f * inputThisFrame.x * Vector2.right, ForceMode2D.Impulse);
        PlayerManagment.timeLeftTillDamage = 0.3f;
        PlayerManagment.invincibilityFromDash = true;
    }

    private void StateControl(){
        if(grounded()){
            currentMoveSpeed = walkSpeed;
            rb.drag = normalDrag;
            rb.gravityScale = 1f;
            if(!groundedLastFrame){
                PlayerSprite.localScale = new Vector3(1.2f,0.8f,1f);
            }
        }
        else{      
            rb.drag = jumpDrag;
            rb.gravityScale = Mathf.Lerp(rb.gravityScale,maxGravity,Time.deltaTime * 3f);
            currentMoveSpeed = Mathf.Lerp(currentMoveSpeed,0.1f,Time.deltaTime);
        }

        groundedLastFrame = grounded();
    }

    private void SpeedControl(){
        if(!grounded()) return;

        if(rb.velocity.x > currentMoveSpeed * currentMoveSpeed){
            rb.velocity = new Vector2(currentMoveSpeed, rb.velocity.y);
        }
    }
}
