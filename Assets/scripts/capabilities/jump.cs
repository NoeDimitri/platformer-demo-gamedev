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


    private Rigidbody2D body;
    private ground ground;
    private Vector2 velocity;

    private int jumpPhase;
    private float defaultGravityscale, jumpSpeed, coyoteCounter;

    private bool desiredJump, onGround, isJumping;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<ground>();

        defaultGravityscale = 1f;
    }

    // Update is called once per frame
    void Update()
    {

        desiredJump |= input.RetrieveJumpInput();

    }

    private void FixedUpdate()
    {
        onGround = ground.getOnGround();
        velocity = body.velocity;

        if(onGround && body.velocity.y == 0)
        {
            jumpPhase = 0;
            coyoteCounter = coyoteTime;
            isJumping= false;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }


        if(desiredJump)
        {
            desiredJump = false;
            JumpAction();
        }

        if(input.RetrieveJumpHoldInput() && body.velocity.y > 0f)
        {
            body.gravityScale = upwardMovementMultiplier;
        }
        else if (input.RetrieveJumpHoldInput() || body.velocity.y < 0f)
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        else if (body.velocity.y == 0f)
        {
            body.gravityScale = defaultGravityscale;
        }

        body.velocity = velocity;
    }

    private void JumpAction()
    {
        if(coyoteCounter >= 0 || jumpPhase < maxAirJumps)
        {
            if(isJumping)
            {
                jumpPhase += 1;
            }

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
}
