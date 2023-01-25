using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    [SerializeField] private inputController input = null;

    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.5f)] private float coyoteTime = 0.1f;
    [SerializeField, Range(0f, 0.5f)] private float jumpBuffer = 0.1f;

    private SpriteRenderer sprite;
    public Color hasDashColor, noDashColor;

    [Header("Dash Attributes")]
    [SerializeField, Range(1f, 50f)] private float dashStrength;
    [SerializeField, Range(0f, 0.5f)] private float dashDuration = 0.1f;
    [SerializeField, Range(0f, 50f)] private float drag;
    [SerializeField, Range(0f, 50f)] private float dragLoss;


    private Rigidbody2D body;
    private ground ground;
    private Vector2 velocity;

    private int jumpPhase;
    private float defaultGravityscale, jumpSpeed, coyoteCounter, jumpBufferCounter;

    private bool desiredJump, onGround, isJumping, canDash, gravityDisabled;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<ground>();
        sprite = GetComponent<SpriteRenderer>();

        defaultGravityscale = 1f;
        desiredJump = false;
        jumpBufferCounter = -0.1f;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {

        desiredJump |= input.RetrieveJumpInput();

    }

    private void FixedUpdate()
    {
        body.drag = Mathf.Max(dragLoss * Time.deltaTime, 0);

        onGround = ground.getOnGround();
        velocity = body.velocity;

        if(onGround && body.velocity.y == 0)
        {
            jumpPhase = 0;
            refreshDash();
            coyoteCounter = coyoteTime;
            isJumping= false;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }


        if (desiredJump)
        {
            desiredJump = false;
            jumpBufferCounter = jumpBuffer;
        }
        else if(!desiredJump && jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if(jumpBufferCounter >= 0)
        {
            JumpAction();
        }

        if(input.RetrieveJumpHoldInput() && body.velocity.y > 0f)
        {
            body.gravityScale = upwardMovementMultiplier;
        }
        else if (!input.RetrieveJumpHoldInput() || body.velocity.y < 0f)
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        else if (body.velocity.y == 0f)
        {
            body.gravityScale = defaultGravityscale;
        }

        if(gravityDisabled)
        {
            body.gravityScale = 0;
        }

        body.velocity = velocity;

        

        if (input.retrieveDashInput() && canDash)
        {

            dash(input.RetrieveMoveInput(), input.RetrieveVerticalInput());

        }
    }

    private void JumpAction()
    {
        if(coyoteCounter >= 0 || jumpPhase < maxAirJumps)
        {
            if(isJumping)
            {
                jumpPhase += 1;
            }

            jumpBufferCounter = -1;
            coyoteCounter = -1;
            isJumping = true;

            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if(velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }
    private void dash(float x, float y)
    {
        canDash = false;
        sprite.color = noDashColor;
        body.gravityScale = 0;
        body.velocity = Vector2.zero;
        body.velocity = new Vector2(x, y).normalized * dashStrength;
        body.drag = drag;
        gravityDisabled = true;

        //if( Mathf.Abs(y) != 1 || Mathf.Abs(x) == 1) 
        StartCoroutine(tempGravityRemoval());
    }

    public void refreshDash()
    {
        sprite.color = hasDashColor;

        canDash = true;

    }

    IEnumerator tempGravityRemoval()
    {
        
        yield return new WaitForSeconds(dashDuration);
        gravityDisabled = false;

    }
}
