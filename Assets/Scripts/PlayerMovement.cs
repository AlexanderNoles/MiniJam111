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
    public float sprintSpeed = 10f;
    public float crouchSpeed = 1f;
    public float normalDrag = 5f;
    private float currentMoveSpeed = 0f;

    [Header("Jump Settings")]
    public float jumpForce = 3f;
    public float jumpDrag = 8f;
    public float maxGravity = 10f;
    public int maxAmountOfJumps = 2;
    private int currentAmountOfJumps;

    [Header("Animation")]
    public Transform PlayerSprite;

    bool grounded(){
        return Physics2D.Raycast(transform.position, Vector2.down, height/2, ground);
    }
    private bool groundedLastFrame = true;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        _instance = this;
    }

    private void Update() {
        inputThisFrame = InputManager.MovementInput();
        SpeedControl();
        StateControl();

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

    private void StateControl(){
        if(grounded()){
            if(InputManager.SprintButtonPressed()){
                currentMoveSpeed = sprintSpeed;
            }
            else{
                currentMoveSpeed = walkSpeed;
            }
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
