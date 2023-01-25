using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shinjingi
{
    public class wallInteractor : MonoBehaviour
    {
        [SerializeField] private inputController controller;

        [Header("Wall Slide")]
        [SerializeField][Range(0.1f, 5f)] private float wallSlideMaxSpeed = 2f;

        [Header("Wall Jump")]
        [SerializeField] private Vector2 wallJumpClimb = new Vector2(4f, 12f);
        [SerializeField] private Vector2 wallJumpBounce = new Vector2(10.7f, 10f);

        private ground collisionDataRetriever;
        private Rigidbody2D body;

        private Vector2 velocity;
        private bool onWall, onGround, desiredJump, wallJumping;
        private float wallDirectionX;

        // Start is called before the first frame update
        void Start()
        {
            collisionDataRetriever = GetComponent<ground>();
            body = GetComponent<Rigidbody2D>();
            //controller = GetComponent<inputController>();

        }

        private void Update()
        {
            if (onWall && !onGround) 
            {
                desiredJump |= controller.RetrieveJumpHoldInput();
            }
        }

        private void FixedUpdate()
        {
            velocity = body.velocity;
            onWall = collisionDataRetriever.onWall;
            onGround = collisionDataRetriever.onGround;
            wallDirectionX = collisionDataRetriever.contactNormal.x;

            #region Wall Slide
            if (onWall)
            {
                if(velocity.y < -wallSlideMaxSpeed)
                {
                    velocity.y = -wallSlideMaxSpeed;
                }

            }
            #endregion

            #region wall jump
            if((onWall && velocity.x == 0) || onGround)
            {
                wallJumping = false;
            }


            if(desiredJump)
            {
                if(-wallDirectionX == controller.RetrieveMoveInput()) 
                {
                    velocity = new Vector2(wallJumpClimb.x * wallDirectionX, wallJumpClimb.y);
                    wallJumping = true;
                    desiredJump = false;
                }
                else if(controller.RetrieveMoveInput() == 0)
                {
                    velocity = new Vector2(wallJumpBounce.x*wallDirectionX, wallJumpBounce.y);
                    wallJumping = true;
                    desiredJump = false;
                }
            }

            #endregion

            body.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collisionDataRetriever.evaluateCollision(collision);

            if(collisionDataRetriever.onWall && !collisionDataRetriever.onGround && wallJumping)
            {
                body.velocity = Vector2.zero;
            }
        }

    }

}
