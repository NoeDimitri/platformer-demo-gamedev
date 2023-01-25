using Shinjingi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(inputController))]

public class move : MonoBehaviour
{

    [SerializeField] private inputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    [SerializeField, Range(0f, 50f)] private float deceleration = 5f;
    [SerializeField, Range(0.05f, 0.5f)] private float wallStickTime = 0.25f;


    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private ground ground;

    private float maxSpeedChange;
    private float acceleration, wallStickCounter;
    private bool onGround;

    private wallInteractor interactor;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<ground>();
        interactor = GetComponent<wallInteractor>();
        
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = input.RetrieveMoveInput();
        //desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.getFriction(), 0f);
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - deceleration, 0f);

    }

    private void FixedUpdate()
    {
        onGround = ground.getOnGround();
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;

        #region wall stick
        if(ground.onWall && !ground.onGround && !interactor.wallJumping)
        {
            if(wallStickCounter > 0)
            {
                velocity.x = 0;
                if(input.RetrieveMoveInput() == ground.contactNormal.x)
                {
                    wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    wallStickCounter = wallStickTime;
                }

            }
            else
            {
                wallStickCounter = wallStickTime;
            }

        }

        #endregion
    }
}
